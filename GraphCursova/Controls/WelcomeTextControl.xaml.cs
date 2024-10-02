using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphCursova.Controls
{
    /// <summary>
    /// Interaction logic for WelcomeTextControl.xaml
    /// </summary>
    public partial class WelcomeTextControl : UserControl
    {
        public WelcomeTextControl()
        {
            InitializeComponent();
            SetupAnimations();
        }
        private void AnimateTextBlock(TextBlock textBlock, string word, double delay, int offset, bool isLastWord)
        {
            textBlock.Text = word;
            textBlock.FontSize = 60;
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.Foreground = Brushes.White;

            var translateYAnimationUp = new DoubleAnimation
            {
                From = 0,
                To = -120 - offset,
                Duration = TimeSpan.FromSeconds(1), 
                BeginTime = TimeSpan.FromSeconds(delay),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            if (isLastWord)
            {
                var SequenceEnd = new Storyboard();
                Storyboard.SetTarget(translateYAnimationUp, textBlock);
                Storyboard.SetTargetProperty(translateYAnimationUp, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
                SequenceEnd.Children.Add(translateYAnimationUp);
                SequenceEnd.Begin();
                return;
            }

            var delayAnimation = new DoubleAnimation
            {
                From = -120 - offset,
                To = -120 - offset, 
                Duration = TimeSpan.FromSeconds(1),
                BeginTime = TimeSpan.FromSeconds(delay + 1) 
            };


            var translateYAnimationDown = new DoubleAnimation
            {
                From = -120 - offset,
                To = -120 - offset - 120,
                Duration = TimeSpan.FromSeconds(1),
                BeginTime = TimeSpan.FromSeconds(delay + 1.5),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };


            var sequence = new Storyboard();
            Storyboard.SetTarget(translateYAnimationUp, textBlock);
            Storyboard.SetTargetProperty(translateYAnimationUp, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
            sequence.Children.Add(translateYAnimationUp);

            Storyboard.SetTarget(delayAnimation, textBlock);
            Storyboard.SetTargetProperty(delayAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
            sequence.Children.Add(delayAnimation);

            Storyboard.SetTarget(translateYAnimationDown, textBlock);
            Storyboard.SetTargetProperty(translateYAnimationDown, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
            sequence.Children.Add(translateYAnimationDown);


            sequence.Begin();
        }



        private void SetupAnimations()
        {
            AnimateTextBlock(word1, "beautiful", 1, 0, false);
            AnimateTextBlock(word2, "modern", 1.5, 0, false);
            AnimateTextBlock(word3, "technological", 3.5, 135, false);
            AnimateTextBlock(word4, "comfortable", 5.5, 245, true);
        }

        
    }
}
   


