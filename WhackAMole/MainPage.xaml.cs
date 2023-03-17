//imports
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhackAMole;

public partial class MainPage : ContentPage
{
    //global variables
    private int _timer, _score;
    private Boolean _timerOn;
    Random _position1, _position2;
    public MainPage()
	{
		InitializeComponent();

        //set global variables and generate randoms
        _position1 = new Random();
        _position2 = new Random();
        _timer = 46;
        _score = 0;
        _timerOn = false;

        //set up timer
        SetUpTimer();

        //set up another repeating method to move mole and stop the game
        UpdaterMethod();
    }

    private void BtnStart_Clicked(object sender, EventArgs e)
    {
        //run start method
        Start();

        //enable reset buttton (disabled until start for functionality reasons)
        BtnReset.IsEnabled = true;
    }

    private void Start()
    {
        //for initialising the mole co-ordinates randomly
        MoveMole();

        //turn timer on
        _timerOn = true;

        //make moles visable
        MolesVisable(true);
    }

    private void Btn4x4_Clicked(object sender, EventArgs e)
    {
        //make 4x4 grid visable and 5x5 grid invisable
        Grid4x4.IsVisible = true;
        Grid5x5.IsVisible = false;
    }

    private void Btn5x5_Clicked(object sender, EventArgs e)
    {
        //make 5x5 grid visable and 4x4 grid invisable
        Grid5x5.IsVisible = true;
        Grid4x4.IsVisible = false;
    }

    private void SetUpTimer()
    {
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(1000), () =>
        {
            Dispatcher.Dispatch(() =>
            {
                TimerFunctions();
            });
            return true;
        });
    }
    private void TimerFunctions()
    {
        if(_timerOn == true && _timer > 0)
        {
            // change the countdown.
            _timer--;
            LblTimer.Text = _timer.ToString();
        }
        else if(_timer == 0)
        {
            //stop the game
            MolesVisable(false);
        }
    }

    private void UpdaterMethod()
    {
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(2500), () =>
        {
            Dispatcher.Dispatch(() =>
            {
                Update();
            });
            return true;
        });
    }

    private void Update()
    {
        //move the mole with this periodic loop (every 2.5 seconds)
        MoveMole();
    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        if (_timer > 0)
        {
            //add 10 points to score
            _score += 10;

            //update score
            LblScore.Text = _score.ToString();
        }

        MoveMole();
    }

    private void BtnReset_Clicked(object sender, EventArgs e)
    {
        //pause the game
        Pause();

        //make prompt visable
        PromptVisable(true);

        //call method to reset everything if yes or resume if no
        if (RdoBtnYes.IsChecked == true)
            Reset();
        else if(RdoBtnNo.IsChecked == true)
        {
            Start();
            PromptVisable(false);
        } 
    }

    private void Reset()
    {
        //stop and restart timer
        _timer = 45;
        LblTimer.Text = _timer.ToString();

        //reset score
        _score = 0;
        LblScore.Text = _score.ToString();

        //make moles invisable
        MolesVisable(false);

        //make prompt invisable
        PromptVisable(false);

        //Disable reset button
        BtnReset.IsEnabled = false;
    }

    private void MoveMole()
    {
        //set row and column values to random
        int r = 0, c = 0;
        if(Grid4x4.IsVisible == true)
        {
            r = _position1.Next(0, 3);
            c = _position2.Next(0, 3);

            //update mole position
            ImgMole4x4.SetValue(Grid.RowProperty, r);
            ImgMole4x4.SetValue(Grid.ColumnProperty, c);
        }
        else
        {
            r = _position1.Next(0, 5);
            c = _position2.Next(0, 5);

            ImgMole5x5.SetValue(Grid.RowProperty, r);
            ImgMole5x5.SetValue(Grid.ColumnProperty, c);
        }
    }

    private void MolesVisable(Boolean Visable)
    {
        if (Visable == true)
        {
            ImgMole4x4.IsVisible = true;
            ImgMole5x5.IsVisible = true;
        }
        else
        {
            ImgMole4x4.IsVisible = false;
            ImgMole5x5.IsVisible = false;
        }
    }

    private void PromptVisable(Boolean Visable)
    {
        if (Visable == true)
        {
            //make reset prompt visable
            ResetPrompt.IsVisible = true;
            LblResetQuestion.IsVisible = true;
        }
        else
        {
            //make reset prompt invisable
            ResetPrompt.IsVisible = false;
            LblResetQuestion.IsVisible = false;

            //make yes button unclicked
            RdoBtnYes.IsChecked = false;
            
            //reset the no nutton
            RdoBtnNo.IsChecked = false;
        }
    }

    private void Pause()
    {
        //stop timer
        _timerOn = false;

        //make moles invisable
        MolesVisable(false);
    }
}