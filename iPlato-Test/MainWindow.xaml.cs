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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iPlato_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml   
    /// </summary>
    ///      
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            
            InitializeComponent();
            ProfessionDataLbl.Visibility = Visibility.Hidden;
            MandatoryLbl.Visibility = Visibility.Hidden;
            LoadDataGridMain();
        }

        private void LoadDataGridMain()
        {
            TextBoxName.Text = "";
            TextBoxDOB.Text = "";
            TextBoxProf.Text = "";
            HiddenId.Text = "0";
            ViewModelData viewModelData = new ViewModelData();

            Test_DBEntities dbEntities = new Test_DBEntities();

            var query = from x in dbEntities.PROFESSIONs
                        select new { x.ID, x.NAME, x.DOB, x.PROFESSION1 };

            List<ProfessionDataGrid> ProfessionList = query.Select(
                                                        c => new ProfessionDataGrid
                                                        {
                                                            ID = c.ID,
                                                            Name = c.NAME,
                                                            DOB = c.DOB,
                                                            Profession = c.PROFESSION1
                                                        }).ToList();
            viewModelData = new ViewModelData
            {
                ProfessionList = ProfessionList
            };

            this.DataContext = viewModelData;
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            int rowID = Convert.ToInt32(HiddenId.Text);
            MandatoryLbl.Visibility = Visibility.Hidden;
            TextBoxName.BorderBrush = Brushes.Gray;
            TextBoxDOB.BorderBrush = Brushes.Gray;
            TextBoxProf.BorderBrush = Brushes.Gray;

            if (TextBoxName.Text.Equals(string.Empty) || TextBoxDOB.Text.Equals(string.Empty) || TextBoxProf.Text.Equals(string.Empty))
            {
                MandatoryLbl.Visibility = Visibility.Visible;
                if (TextBoxName.Text.Equals(string.Empty))
                    TextBoxName.BorderBrush = Brushes.Red;
                if (TextBoxDOB.Text.Equals(string.Empty))
                    TextBoxDOB.BorderBrush = Brushes.Red;
                if (TextBoxProf.Text.Equals(string.Empty))
                    TextBoxProf.BorderBrush = Brushes.Red;

                return;
            }

            string textBoxName = TextBoxName.Text;
            string textBoxDOB = TextBoxDOB.Text;
            string textBoxProf = TextBoxProf.Text;

            ViewModelData viewModelData = new ViewModelData();

            viewModelData = new ViewModelData
            {
                ProfessionList = new List<ProfessionDataGrid>()
                    {
                        new ProfessionDataGrid
                        {
                            Name = textBoxName,
                            DOB = textBoxDOB,
                            Profession = textBoxProf,
                        },
                    }
            };

            Test_DBEntities dbEntities = new Test_DBEntities();

            if (rowID==0)
            {
                PROFESSION dbInsert = new PROFESSION();
                //dbInsert.ID = dbInsert.ID + 1;
                dbInsert.NAME = textBoxName;
                dbInsert.DOB = textBoxDOB;
                dbInsert.PROFESSION1 = textBoxProf;
                dbEntities.PROFESSIONs.Add(dbInsert);
                dbEntities.SaveChanges();
            }
            else
            {
 
                PROFESSION dbUpdate = dbEntities.PROFESSIONs.First(p => p.ID == rowID);
                dbUpdate.NAME = textBoxName;
                dbUpdate.DOB = textBoxDOB;
                dbUpdate.PROFESSION1 = textBoxProf;
                dbEntities.SaveChanges();
            }

            LoadDataGridMain();
        }

        private void DataGridMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProfessionDataGrid selectedRow = (ProfessionDataGrid)DataGridMain.SelectedItem;
            
            if (selectedRow != null)
            {
                ProfessionDataLbl.Visibility = Visibility.Visible;
                ProfessionDataLbl.Content = "Profession: " + selectedRow.Profession;
            }
            else
            {
                ProfessionDataLbl.Visibility = Visibility.Hidden;
                MandatoryLbl.Visibility = Visibility.Hidden;
            }
        }

        private void onClickDelete(object sender, RoutedEventArgs e)
        {
            ProfessionDataGrid selectedRow = (ProfessionDataGrid)DataGridMain.SelectedItem;
            Test_DBEntities dbEntities = new Test_DBEntities();

            PROFESSION dbDelete = new PROFESSION();

            dbDelete.ID = selectedRow.ID;
            dbEntities.PROFESSIONs.Attach(dbDelete);
            dbEntities.PROFESSIONs.Remove(dbDelete);
            dbEntities.SaveChanges();

            LoadDataGridMain();
        }

        private void onClickEdit(object sender, RoutedEventArgs e)
        {
            ProfessionDataGrid selectedRow = (ProfessionDataGrid)DataGridMain.SelectedItem;

            if (selectedRow != null)
            {
                ProfessionDataLbl.Visibility = Visibility.Visible;
                HiddenId.Text = selectedRow.ID.ToString();
                TextBoxName.Text = selectedRow.Name;
                TextBoxDOB.Text = selectedRow.DOB;
                TextBoxProf.Text = selectedRow.Profession;
            }

        }

    }
}
