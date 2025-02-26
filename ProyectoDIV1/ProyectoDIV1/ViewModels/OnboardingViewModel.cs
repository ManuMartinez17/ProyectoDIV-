﻿using ProyectoDIV1.Models;
using ProyectoDIV1.Views;
using ProyectoDIV1.Views.Account;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    public class OnboardingViewModel : BaseViewModel
    {
        private ObservableCollection<OnboardingModel> items;
        private int position;
        private string skipButtonText;

        public OnboardingViewModel()
        {
            SetSkipButtonText("SALTAR");
            InitializeOnBoarding();
            InitializeSkipCommand();
        }
        public ICommand SkipCommand { get; private set; }
        private void SetSkipButtonText(string skipButtonText)
                => SkipButtonText = skipButtonText;

        private void InitializeOnBoarding()
        {
            Items = new ObservableCollection<OnboardingModel>
            {
                new OnboardingModel
                {
                    Title = "Bienvenido a \n DIV",
                    Content = "Aplicativo para certificar horas de trabajo.",
                    ImageUrl = "ilust1.svg"
                },
                new OnboardingModel
                {
                    Title = "Registrate como Candidato y/o Empresa",
                    Content = " hacer uso de tus habilidades y profesión, para candidatos o empresas que requieran un trabajo.",
                    ImageUrl = "ilust2.svg"
                },
                new OnboardingModel
                {
                    Title = "Califica y Notifica",
                    Content = "Califica y notifica a candidatos y/o empresas.",
                    ImageUrl = "ilust3.svg"
                }
            };
        }

        private void InitializeSkipCommand()
        {
            SkipCommand = new Command(() =>
            {
                if (LastPositionReached())
                {
                    ExitOnBoarding();
                }
                else
                {
                    MoveToNextPosition();
                }
            });
        }

        private async static void ExitOnBoarding()
        {
            try
            {
                await PopupNavigation.Instance.PushAsync(new PopupLoadingPage());
                Application.Current.MainPage = new MasterPage();
                await Shell.Current.GoToAsync(nameof(LoginPage));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                await PopupNavigation.Instance.PopAsync();
            }

        }


        private void MoveToNextPosition()
        {
            var nextPosition = ++Position;
            Position = nextPosition;
        }

        private bool LastPositionReached()
            => Position == Items.Count - 1;

        public ObservableCollection<OnboardingModel> Items
        {
            get => items;
            set => SetProperty(ref items, value);
        }

        public string SkipButtonText
        {
            get => skipButtonText;
            set => SetProperty(ref skipButtonText, value);
        }

        public int Position
        {
            get => position;
            set
            {
                if (SetProperty(ref position, value))
                {
                    UpdateSkipButtonText();
                }
            }
        }

        private void UpdateSkipButtonText()
        {
            if (LastPositionReached())
            {
                SetSkipButtonText("ENTENDIDO");
            }
            else
            {
                SetSkipButtonText("SALTAR");
            }
        }


    }
}
