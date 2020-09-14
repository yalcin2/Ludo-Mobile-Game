"""
Task 3

Yalcin Formosa
MSD6.3A
"""

import cv2
import numpy as np

camera = cv2.VideoCapture(0)

face_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + "haarcascade_frontalface_default.xml")

while True:
    _, img = camera.read()

    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)

    faces = face_cascade.detectMultiScale(
        gray,
        scaleFactor=1.1,
        minNeighbors=5,
        minSize=(30, 30),
        flags=cv2.CASCADE_SCALE_IMAGE
    )

    for (x, y, w, h) in faces:
        counter = counter + 1
        cv2.rectangle(img, (x, y), (x + w, y + h), (0, 255, 0), 2)
        cv2.rectangle(img, (x - 40, y - 90), (x + w + 40, y + h + 90), (0, 255, 0), 2)

        roi = img[y-90:y+h+90, x-40:x+w+40]

    cv2.imshow("image", img)

    key = cv2.waitKey(1)

    if key == 27:
        break
    elif key == 32:
        cv2.imwrite("pictureSaved%d.png" % i, roi)

camera.release()
cv2.destroyAllWindows()
