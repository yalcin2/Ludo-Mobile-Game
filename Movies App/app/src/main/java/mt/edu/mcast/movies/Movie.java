package mt.edu.mcast.movies;

import android.graphics.Bitmap;

public class Movie {

    private String Title;
    private String id;
    private String year;
    private Bitmap poster;

    public String getTitle() {
        return Title;
    }

    public void setTitle(String title) {
        Title = title;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getYear() {
        return year;
    }

    public void setYear(String year) {
        this.year = year;
    }

    public Bitmap getPoster() {
        return poster;
    }

    public void setPoster(Bitmap poster) {
        this.poster = poster;
    }



    public Movie(String title, String id, String year, Bitmap poster){
        this.Title = title;
        this.id  = id;
        this.year = year;
        this.poster = poster;
    }




}
