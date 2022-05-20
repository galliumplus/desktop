﻿using System;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gallium_v1.Vue
{
    /// <summary>
    /// Logique d'interaction pour ConnexionFenetre.xaml
    /// </summary>
    public partial class ConnexionFenetre : Window
    {
        public ConnexionFenetre()
        {
            InitializeComponent();
            Username.Focus();

        }

        /// <summary>
        /// Lance la méthode pour se connecter quand on appuie sur une touche 
        /// </summary>
        /// <param name="key"></param>
        private void EntreeDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.ConnexionToAccount();
            }
        }

        /// <summary>
        /// Connecte au compte si le bouton est cliqué
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.ConnexionToAccount();
        }

        /// <summary>
        /// Méthode qui permet de se connecter et d'ouvrir une nouvelle fenêtre si la connection est réussi
        /// </summary>
        private void ConnexionToAccount()
        {
            GalliumFenetre gallium = new GalliumFenetre();
            this.Close();
            gallium.Show();
        }
    }
}
