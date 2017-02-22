using SimpleNuclearWarGameplayDemo.Game;
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
    public partial class MainForm : Form
    {
        GameSet gameSet;
        GameControl game;
        NewsForm news = new NewsForm();

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gameSet = Data.Serializer.Load(Data.Serializer.Filename);
            if(gameSet == null)
            {
                MessageBox.Show("Unable to load '" + Data.Serializer.Filename.FullName 
                    + "'. Will create default content at it's place.","Nuclear War Game Data");
                gameSet = Data.SampleData.GenerateDemoData();
                Data.Serializer.SaveAll(gameSet);
            }                 
                   
            game = new GameControl(gameSet);
            foreach (Nation nation in game.GetPlayableNations())
            {
                comboBoxNations.Items.Add(nation.Name);
            }
            comboBoxNations.SelectedIndex = 0;
        }

        private void TargetHit(object sender, EventArgs e)
        {
            if(sender is AttackAction && e is TargetReachedEventArgs)
            {                
                AttackAction attack = (AttackAction) sender;
                TargetReachedEventArgs target = (TargetReachedEventArgs)e;                
                game.Hit(target.Region, attack, news);                
                game.Refresh(news);
            }
            UpdatePlayerLists();
        }

        private Entities.Region AskForTargetIfNecessary(Entities.Action action, World world)
        {
            if (action is Attack)
            {
                Attack attack = (Attack)action;
                // some attacks have a fixed region
                if (String.IsNullOrEmpty(attack.TargetRegion))
                {
                    return AskForTarget(world); 
                }
                return world.Find(attack.TargetRegion);
            }
            // a defence doesn't need a region
            return null;
        }

        private Entities.Region AskForTarget(World world)
        {
            TargetSelectorForm targetSelector = new TargetSelectorForm();
            targetSelector.SetWorld(world);
            if(targetSelector.ShowDialog() == DialogResult.OK) {
                return targetSelector.getSelectedTarget();
            }
            return GameControl.GetRandom(world.Regions);

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            game?.StopAIs();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            var playableNation = game.GetNation((string)comboBoxNations.SelectedItem);
            comboBoxNations.Enabled = false;
            game.SelectPlayerNation(playableNation);
            news.DisplayHtml(game.GetIntroduction());
            game.StartAIs(TargetHit, news);
            news.Show();
            buttonStart.Enabled = false;
            UpdatePlayerLists();
        }

        private void UpdatePlayerLists()
        {
            lock(this) { 
                try {
                    UpdatePopulation();
                    listBoxAvailable.Items.Clear();
                    foreach (Entities.Action action in game.Player.GetAvailableActions())
                    {
                        if (action.Quantity > 0)
                        {
                            listBoxAvailable.Items.Add(action.Name);
                        }                        
                    }
                    listBoxSelected.Items.Clear();
                    foreach (Entities.Action action in game.Player.GetActiveActions())
                    {
                        listBoxSelected.Items.Add(action.Name);
                    }
                    labelActiveCount.Text = game.Player.GetNation().Name + " has " + listBoxSelected.Items.Count + "/" + game.Player.GetNation().ActionLimit + " active.";
                } catch( Exception ex)
                {
                    Console.Out.Write(ex.Message);
                }
            }
        }

        private void UpdatePopulation()
        {
            lock(labelPopulation)
            {
                if (gameSet != null)
                {
                    labelPopulation.Text = "Total world population: " + gameSet.World.GetTotalPopulation();
                }
            }            
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            try
            {
                Entities.Action action = game.Player.GetAvailableAction((string)listBoxAvailable.SelectedItem);
                if(action.Quantity > 0)
                {
                    lock (this)
                    {
                        action.TargetReached -= TargetHit;
                        action.TargetReached += TargetHit;
                    }
                    bool success = game.Player.Trigger(action, AskForTargetIfNecessary(action, gameSet.World), news);
                }
            }
            catch (Exception ex)
            {
                Console.Out.Write(ex.Message);
            }            
            UpdatePlayerLists();
        }

        private void timerRefesh_Tick(object sender, EventArgs e)
        {
            UpdatePopulation();
        }
    }
}

