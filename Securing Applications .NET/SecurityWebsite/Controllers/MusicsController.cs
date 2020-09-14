using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Web.Mvc;
using Common;
using BusinessLogic;
using System.IO;

namespace SecurityWebsite.Controllers
{
    [Authorize]
    public class MusicsController : Controller
    {

        // GET: Musics
        //[Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var listForAdmin = new MusicsBL().GetMusics();
                return View(listForAdmin);
            }
            else {
                string currentUser = User.Identity.Name;
                Guid currentID = new UsersBL().GetUserID(currentUser);

                var listForUser = new MusicsBL().GetMusicWithID(currentID);
                return View(listForUser);
            }
        }

        public ActionResult Details(string id)
        {
            //decrypting the id
            try
            {
                int originalId = Convert.ToInt32(Encryption.DecryptQueryString(id));

                var myMusic = new MusicsBL().GetMusic(originalId);
                return View(myMusic);

            }
            catch(Exception ex)
            {
                Logger.LogMessage("", Request.Path, "Error: " + ex.Message);
                TempData["errormessage"] = "ERROR: Cannot be navigated too, please try again later.";
                return RedirectToAction("Index");
            }
        }
        //[Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles ="Admin")]
        public ActionResult Create(Music m, HttpPostedFileBase fileData)
        {
            try
            {
                Logger.LogMessage("", Request.Path, "Entered the Create Action");

                string filePath = fileData.FileName;
                string filename = Path.GetFileName(filePath);
                string ext = Path.GetExtension(filename);
                string contenttype = String.Empty;
                Stream checkStream = fileData.InputStream;
                BinaryReader chkBinary = new BinaryReader(checkStream);
                Byte[] chkbytes = chkBinary.ReadBytes(0x10);

                string data_as_hex = BitConverter.ToString(chkbytes);
                string magicCheck = data_as_hex.Substring(0, 11);

                switch (magicCheck)
                {
                    case "52-49-46-46":
                        contenttype = "audio/x-wav";
                        break;
                }
                if (contenttype != String.Empty)
                {
                    if (fileData.ContentLength < 8 * 1024 * 1024)
                    {
                        Byte[] bytes = chkBinary.ReadBytes((Int32)checkStream.Length);
                        string uniqueFilename = Guid.NewGuid() + Path.GetExtension(fileData.FileName);
                        string absolutePath = Server.MapPath(@"\App_Data") + @"\";
                        fileData.SaveAs(absolutePath + uniqueFilename);

                        string currentUser = User.Identity.Name;
                        Guid currentID = new UsersBL().GetUserID(currentUser);

                        string publicKey = new UsersBL().GetUserPublicKey(currentID);
                        MemoryStream myEncryptedfile = Encryption.HybridEncrypt(fileData.InputStream, publicKey);

                        string privateKey = new UsersBL().GetUserPrivateKey(currentID);
                        string signature = Encryption.SignFile(myEncryptedfile.ToArray(), privateKey);

                        System.IO.File.WriteAllBytes(absolutePath + uniqueFilename, myEncryptedfile.ToArray());

                        m.MusicPath = @"\App_Data\" + uniqueFilename;

                        new MusicsBL().AddMusic(m.Name, m.Description, m.Genre_fk, currentID, m.MusicPath, signature);
                        Logger.LogMessage("", Request.Path, "Finished adding the item to the database");

                        TempData["message"] = "Music file added successfully";
                        Logger.LogMessage("", Request.Path, "Music file added successfully");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["errormessage"] = "Music file is greater than 8 MB";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["errormessage"] = "Invalid file type";
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                Logger.LogMessage("", Request.Path, "Error: " + ex.Message);
                TempData["errormessage"] = "Your music file was not added! Try again later";
                return View(m);

            }

        }


        //[Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            try
            {
                int originalId = Convert.ToInt32(Encryption.DecryptQueryString(id));
                new MusicsBL().DeleteMusic(originalId);
                Logger.LogMessage("", Request.Path, "Music deleted successfully");
                TempData["message"] = "Music deleted successfully";
            }
            catch (Exception ex)
            {
                //log
                Logger.LogMessage("", Request.Path, "Error: " + ex.Message);
                TempData["errormessage"] = "Music was not deleted! Try again later";
            }

            return RedirectToAction("Index");
        }


        public ActionResult Download(string id)
        {
            try
            {
                int originalId = Convert.ToInt32(Encryption.DecryptQueryString(id));
                var myMusic = new MusicsBL().GetMusic(originalId);

                if (myMusic.MusicPath != null)
                {
                    byte[] myFileBytes = System.IO.File.ReadAllBytes(
                         Server.MapPath(myMusic.MusicPath));

                    MemoryStream encryptedFile = new MemoryStream(myFileBytes);

                    // NOTE the publickey and privatekey has to be the key of the file owner
                    string publicKey = new UsersBL().GetUserPublicKey(myMusic.User_fk);
                    string privateKey = new UsersBL().GetUserPrivateKey(myMusic.User_fk);

                    string signature = myMusic.Signature;

                    bool result = Encryption.VerifyFile(myFileBytes, publicKey, signature);

                    if (result == false)
                    {
                        throw new Exception("Invalid file!");
                    }
                    else
                    {
                        MemoryStream decryptedFile =Encryption.HybridDecrypt(encryptedFile, privateKey);
                        return File(decryptedFile.ToArray(), System.Net.Mime.MediaTypeNames.Application.Octet
                          , Path.GetFileName(myMusic.MusicPath));
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.LogMessage("", Request.Path, "Error: " + ex.Message);
                TempData["errormessage"] = "Music was not downloaded! Try again later";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult ShareMusic()
        {
            if (User.IsInRole("Admin"))
            {
                var listForAdmin = new MusicsBL().GetMusics();
                return View(listForAdmin);
            }
            else
            {
                string currentUser = User.Identity.Name;
                Guid currentID = new UsersBL().GetUserID(currentUser);

                var listForUser = new MusicsBL().GetMusicWithID(currentID);
                return View(listForUser);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShareMusic(Permission p)
        {
            UsersBL u = new UsersBL();
            MusicsBL m = new MusicsBL();

            //bool permission = true;

            return View();

            /*
            try
            {
                if (email != null || u.GetUser(email) != null)
                {
                    if (email != User.Identity.Name)
                    {
                        Guid sharedUser = u.GetUserID(email);
                        p.AddPermission(permission, musicId, sharedUser);
                        TempData["message"] = "Music has been shared";
                        return View();
                    }
                    else
                    {
                        TempData["errormessage"] = "ERROR: You cannot share the entry with yourself";
                        return View();
                    }
                }
                else
                {
                    TempData["errormessage"] = "ERROR: Do not leave the fields empty";
                    return View();
                }
            }
            catch(Exception ex)
            {
                //if(ex.Message = ""){ TempData["errormessage"] = ex.message; }
                TempData["errormessage"] = ex;
                return View();
            }
            */
        }

    }
}