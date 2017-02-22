using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleNuclearWarGameplayDemo.Entities;

namespace SimpleNuclearWarGameplayDemo
{
    public partial class TargetSelectorForm : Form
    {
        private World world;
        private Entities.Region selectedRegion;

        public TargetSelectorForm()
        {
            InitializeComponent();
        }

        public void SetWorld(World world)
        {           
            this.world = world;            
        }

        public Entities.Region getSelectedTarget()
        {            
            return selectedRegion;            
        }

        private void TargetSelectorForm_Load(object sender, EventArgs e)
        {
            foreach (Entities.Region region in this.world.Regions)
            {
                this.listViewRegions.Items.Add(
                    new ListViewItem(new string[] { region.Name, region.Population.ToString() }));
            }
        }

        private void listViewRegions_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            try {
                this.selectedRegion = world.Find(listViewRegions.SelectedItems[0].Text);
                this.DialogResult = DialogResult.OK;
            } catch
            {                
                this.DialogResult = DialogResult.Abort;
            }
            this.Close();
        }
    }
}
