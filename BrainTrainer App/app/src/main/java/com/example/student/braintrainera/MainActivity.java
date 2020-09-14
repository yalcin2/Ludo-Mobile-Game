package com.example.student.braintrainera;

import android.os.CountDownTimer;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import java.net.Inet4Address;
import java.util.ArrayList;
import java.util.Random;

public class MainActivity extends AppCompatActivity {

    TextView sumTextView;
    TextView scoreTextView;
    TextView messageTextView;
    TextView txtTimer;
    Button btnTopLeft;
    Button btnTopRight;
    Button btnBottomLeft;
    Button btnBottomRight;
    ArrayList<Integer> answers = new ArrayList<Integer>();
    int correctAnswer;
    int locationOfCorrectAnswer;
    int score;
    int numberOfQuestions;
    CountDownTimer countDownTimer;
    Button btnPlayAgain;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        sumTextView = findViewById(R.id.txtSum);
        scoreTextView = findViewById(R.id.txtPoints);
        messageTextView = findViewById(R.id.txtMessage);
        txtTimer = findViewById(R.id.txtTimer);
        btnTopLeft = findViewById(R.id.btnTopLeft);
        btnTopRight = findViewById(R.id.btnTopRight);
        btnBottomLeft = findViewById(R.id.btnBottomLeft);
        btnBottomRight = findViewById(R.id.btnBottomRight);
        btnPlayAgain = findViewById(R.id.btnPlayAgain);
        score = 0;
        numberOfQuestions = 0;

        messageTextView.setText("");
        //LoadNextQuestion();
        StartGame();
    }

    private void StartGame(){
        score = 0;
        numberOfQuestions = 0;
        messageTextView.setText("");
        scoreTextView.setText("0/0");
        LoadNextQuestion();

        //start the timer
        countDownTimer = new CountDownTimer(30000,1000) {
            @Override
            public void onTick(long millisUntilFinished) {
                txtTimer.setText(String.valueOf(millisUntilFinished/1000) + "s");
            }

            @Override
            public void onFinish() {
                messageTextView.setText("Final Score:" + Integer.toString(score));
                txtTimer.setText("0s");
                btnPlayAgain.setVisibility(View.VISIBLE);
                SetEnabledButtons(false);

            }
        }.start();

    }

    private void SetEnabledButtons(boolean isEnabled){
        btnTopLeft.setEnabled(isEnabled);
        btnTopRight.setEnabled(isEnabled);
        btnBottomLeft.setEnabled(isEnabled);
        btnBottomRight.setEnabled(isEnabled);
    }


    private void LoadNextQuestion(){
        numberOfQuestions = numberOfQuestions + 1;
        answers = new ArrayList<Integer>();
        Random random = new Random();
        int a = random.nextInt(10);
        int b = random.nextInt(10);
        sumTextView.setText(Integer.toString(a) + "+" + Integer.toString(b));
        correctAnswer = a+b;
        locationOfCorrectAnswer = random.nextInt(4); //0,1,2,3
        int randomIncorrectAnswer;
        for(int i=0; i<4;i++){
            if(i == locationOfCorrectAnswer){
                answers.add(correctAnswer);
            }
            else
            {
                //generate randomly an incorrect answer and add inside the array
                randomIncorrectAnswer = random.nextInt(50);
                if(randomIncorrectAnswer ==correctAnswer){
                    while(true){
                        randomIncorrectAnswer = random.nextInt(50);
                        if(randomIncorrectAnswer != correctAnswer)
                        {
                            break;
                        }
                    }
                }
                answers.add(randomIncorrectAnswer);
            }
        }

        btnTopLeft.setText(Integer.toString(answers.get(0)));
        btnTopRight.setText(Integer.toString(answers.get(1)));
        btnBottomLeft.setText(Integer.toString(answers.get(2)));
        btnBottomRight.setText(Integer.toString(answers.get(3)));
    }

    public void ChooseAnswer(View view){
        Button buttonClicked = (Button)view;
        Log.i("Button Clicked",buttonClicked.getTag().toString());
        int tagButton = Integer.parseInt(buttonClicked.getTag().toString());
        if(locationOfCorrectAnswer == tagButton){
            //correct answer
            score = score + 1;
            messageTextView.setText("Correct Answer");
        }
        else
        {
            messageTextView.setText("Incorrect Answer");
        }

        ShowScore();
        LoadNextQuestion();
    }

    private void ShowScore(){
        scoreTextView.setText(score + "/" + numberOfQuestions);
    }

    public void PlayAgain(View view){
        StartGame();
        SetEnabledButtons(true);
        btnPlayAgain.setVisibility(View.INVISIBLE);
    }
}
