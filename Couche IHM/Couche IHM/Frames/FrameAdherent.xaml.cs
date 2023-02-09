﻿using Couche_Métier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Couche_IHM.Frames
{
    /// <summary>
    /// Logique d'interaction pour FrameAdherent.xaml
    /// </summary>
    public partial class FrameAdherent : Page
    {
        // Représente le manager des adhérents
        private AdhérentManager adhérentManager;

        
        /// <summary>
        /// Cosntructeur du frame adhérent
        /// </summary>
        /// <param name="adhérentManager">manager des adhérents</param>
        public FrameAdherent(AdhérentManager adhérentManager)
        {
            InitializeComponent();
            this.adhérentManager = adhérentManager;
            infoAdherent.Visibility = Visibility.Hidden;

            // Met à jour l'affichage
            UpdateView();

            // Focus l'utilisateur sur la barre de recherche
            this.rechercheAcompte.Focus();

            this.listadherents.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Id", System.ComponentModel.ListSortDirection.Ascending));


        }


        /// <summary>
        /// Permet de mettre à jour la liste des adhérents
        /// </summary>
        private void UpdateView()
        {
            listadherents.ItemsSource = null;
            listadherents.ItemsSource = adhérentManager.GetAdhérents();
        }


        /// <summary>
        /// Permet d'afficher les informations d'un adhérent 
        /// </summary>
        /// <param name="infoUser"> Information de l'utilisateur </param>
        private void AfficheAcompte(string infoUser)
        {
            Adhérent adhérent = this.adhérentManager.GetAdhérent(infoUser);
            if (adhérent != null)
            {
                AfficheAcompte(adhérent);

            }
            else
            {
                infoAdherent.Visibility = Visibility.Hidden;
                this.listadherents.SelectedItem = null;
                this.buttonValidate.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Permet d'afficher lesi nformations d'un adhérent
        /// </summary>
        /// <param name="adhérent">adhérent à détailler</param>
        private void AfficheAcompte(Adhérent adhérent)
        {
            this.id.Text = adhérent.Id;
            this.argent.Text = Convert.ToString(adhérent.ArgentIHM);
            this.name.Text = adhérent.NomCompletIHM;
            if (adhérent.CanPass == true)
            {
                this.ouibypass.IsChecked = true;
            }
            else
            {
                this.nonbypass.IsChecked = true;
            }

            this.buttonValidate.Visibility = Visibility.Hidden;
            ResetWarnings();
        }


        /// <summary>
        /// Permet de sélectionner un adhérent quand l'utilisateur clique sur une ligne de la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectAdherent(object sender, SelectionChangedEventArgs e)
        {
            Adhérent adhérent =  (Adhérent)this.listadherents.SelectedItem;
            if (adhérent != null)
            {
                AfficheAcompte(adhérent);
                infoAdherent.Visibility = Visibility.Visible;
            }
        }


        /// <summary>
        /// Permet d'afficher un accompte en le recherchant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchAdherent(object sender, TextChangedEventArgs e)
        {
            if(this.rechercheAcompte.Text != "" && this.rechercheAcompte.Text != " ")
            {
                infoAdherent.Visibility = Visibility.Visible;
                AfficheAcompte(this.rechercheAcompte.Text);
            }
            else
            {
                infoAdherent.Visibility = Visibility.Hidden;
                this.listadherents.SelectedItem = null;
                this.buttonValidate.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Permet de cacher les warnings
        /// </summary>
        private void ResetWarnings()
        {
            this.compteWarning.Visibility = Visibility.Hidden;
            this.identiteWarning.Visibility = Visibility.Hidden;
            this.argentWarning.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Permet de valider les changements faits à un utilisateur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValiderChangements(object sender, RoutedEventArgs e)
        {
            ResetWarnings();

            try
            {
                // Mise à jour du nom et du prénom
                string nomAdherent;
                string prenomAdherent;
                if (this.name.Text.Contains(" "))
                {
                    string[] nomComplet = this.name.Text.Split(" ");
                    nomAdherent = nomComplet[0];
                    prenomAdherent = nomComplet[1];
                }
                else
                {
                    throw new Exception("IdentiteFormat");
                }


                // Mise à jour de l'id
                string id;
                if (this.id.Text[0] == prenomAdherent.ToLower()[0] && this.id.Text[1] == nomAdherent.ToLower()[0])
                {
                    id = this.id.Text;
                }
                else
                {
                    throw new Exception("IDFormat");
                }

                // Mise à jour du bypass
                bool adherentCanPass = this.ouibypass.IsChecked.HasValue && this.ouibypass.IsChecked.Value;


                // Mise à jour de l'argent
                float argentFinal;
                string argentFormat = this.argent.Text.Replace(".", ",");
                argentFormat = argentFormat.Replace("€", " ");
                argentFormat = argentFormat.Trim();
                if (new Regex("^[0-9]+$").IsMatch(argentFormat) || new Regex("^[0-9]+,[0-9]+$").IsMatch(argentFormat))
                {
                    argentFinal = (float)Convert.ToDouble(argentFormat);
                }
                else
                {
                    throw new Exception("ArgentFormat");
                }

                // Mise à jour de l'adhérent
                this.adhérentManager.UpdateAdhérent(new Adhérent(id, nomAdherent, prenomAdherent, argentFinal, adherentCanPass));

                // Refresh vue
                UpdateView();
                infoAdherent.Visibility = Visibility.Hidden;
                this.listadherents.SelectedItem = null;
                this.buttonValidate.Visibility = Visibility.Hidden;
            }
            catch(Exception ex)
            {
                switch (ex.Message)
                {
                    case "ArgentFormat":
                        this.argentWarning.Visibility = Visibility.Visible;
                        break;
                    case "IDFormat":
                        this.compteWarning.Visibility = Visibility.Visible;
                        break;
                    case "IdentiteFormat":
                        this.identiteWarning.Visibility = Visibility.Visible;
                        break;
                }
            }
            
        }

        /// <summary>
        /// Permet d'afficher le bouton de validation des changements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowValidationButton(object sender, TextChangedEventArgs e)
        {
            this.buttonValidate.Visibility = Visibility.Visible;
        }


        /// <summary>
        /// Permet d'afficher le bouton de validation des changements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowValidationButton(object sender, RoutedEventArgs e)
        {
            this.buttonValidate.Visibility = Visibility.Visible;
        }
    }
}
