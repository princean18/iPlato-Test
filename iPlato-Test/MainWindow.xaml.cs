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
            ProfessionLbl.Visibility = Visibility.Hidden;
            ProfessionDataLbl.Visibility = Visibility.Hidden;
            LoadDataGridMain();
        }

        private void LoadDataGridMain()
        {
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

            PROFESSION dbInsert = new PROFESSION();
            //dbInsert.ID = dbInsert.ID + 1;
            dbInsert.NAME = textBoxName;
            dbInsert.DOB = textBoxDOB;
            dbInsert.PROFESSION1 = textBoxProf;
            dbEntities.PROFESSIONs.Add(dbInsert);
            dbEntities.SaveChanges();

            LoadDataGridMain();
        }

        private void DataGridMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProfessionDataGrid selectedRow = (ProfessionDataGrid)DataGridMain.SelectedItem;
            
            if (selectedRow != null)
            {
                ProfessionLbl.Visibility = Visibility.Visible;
                ProfessionDataLbl.Visibility = Visibility.Visible;
                ProfessionDataLbl.Content = selectedRow.Profession;
            }
            else
            {
                ProfessionLbl.Visibility = Visibility.Hidden;
                ProfessionDataLbl.Visibility = Visibility.Hidden;
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
    }
}
