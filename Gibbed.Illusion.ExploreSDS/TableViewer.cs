﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Gibbed.Illusion.ExploreSDS
{
    public partial class TableViewer : Form
    {
        private Dictionary<uint, string> ColumnNames = new Dictionary<uint, string>();

        public TableViewer()
        {
            this.InitializeComponent();

            // TODO: move this big list to an external file
            // I am lazy :effort:
            this.AddColumnName("0");
            this.AddColumnName("1");
            this.AddColumnName("2");
            this.AddColumnName("3");
            this.AddColumnName("4");
            this.AddColumnName("5");
            this.AddColumnName("6");
            this.AddColumnName("Id");
            this.AddColumnName("ID");
            this.AddColumnName("Top");
            this.AddColumnName("Max");
            this.AddColumnName("flag");
            this.AddColumnName("guid");
            this.AddColumnName("HP");
            this.AddColumnName("CO");
            this.AddColumnName("Type");
            this.AddColumnName("Left");
            this.AddColumnName("name");
            this.AddColumnName("Name");
            this.AddColumnName("Flags");
            this.AddColumnName("Speed");
            this.AddColumnName("Weight");
            this.AddColumnName("model");
            this.AddColumnName("scale");
            this.AddColumnName("speed");
            this.AddColumnName("Descr");
            this.AddColumnName("Group");
            this.AddColumnName("Notes");
            this.AddColumnName("Price");
            this.AddColumnName("Range");
            this.AddColumnName("Right");
            this.AddColumnName("Sound");
            this.AddColumnName("Value");
            this.AddColumnName("Water");
            this.AddColumnName("Bottom");
            this.AddColumnName("Default");
            this.AddColumnName("Defence");
            this.AddColumnName("Description");
            this.AddColumnName("impact");
            this.AddColumnName("Material");
            this.AddColumnName("Possibility");
            this.AddColumnName("Reaction");
            this.AddColumnName("Visibility");
            this.AddColumnName("restitution");
            this.AddColumnName("FadeIn");
            this.AddColumnName("FadeOut");
            this.AddColumnName("fragtex");
            this.AddColumnName("Impact");
            this.AddColumnName("SlipId");
            this.AddColumnName("TexCols");
            this.AddColumnName("TexEnd");
            this.AddColumnName("TexRows");
            this.AddColumnName("TexStart");
            this.AddColumnName("TypeName");
            this.AddColumnName("BlockId");
            this.AddColumnName("CrashId");
            this.AddColumnName("MaterialId");
            this.AddColumnName("RideId");
            this.AddColumnName("SlideId");
            this.AddColumnName("MaterialName");
            this.AddColumnName("CrossType");
            this.AddColumnName("affector");
            this.AddColumnName("MaxDistance");
            this.AddColumnName("BlockingTime");
        }

        private void AddColumnName(string name)
        {
            this.ColumnNames.Add(FileFormats.FNV.Hash32(name), name);
        }

        public void LoadFile(FileFormats.SdsMemory.Entry entry)
        {
            var tables = new FileFormats.ResourceTypes.TableResource();
            tables.Deserialize(entry.Header, entry.Data);

            this.comboBox.Items.Clear();
            foreach (var table in tables.Tables)
            {
                this.comboBox.Items.Add(table);
            }
            if (this.comboBox.Items.Count > 0)
            {
                this.comboBox.SelectedIndex = 0;
            }
        }

        private void OnLoadFromFile(object sender, EventArgs e)
        {
        }

        private void OnSelectTable(object sender, EventArgs e)
        {
            var table = this.comboBox.SelectedItem as FileFormats.ResourceTypes.TableData;

            this.dataGridView.Columns.Clear();
            this.dataGridView.Rows.Clear();

            foreach (var column in table.Columns)
            {
                string name;

                if (this.ColumnNames.ContainsKey(column.NameHash) == true)
                {
                    name = this.ColumnNames[column.NameHash];
                }
                else
                {
                    name = column.NameHash.ToString("X8");
                }

                this.dataGridView.Columns.Add(name, name);
            }

            foreach (var rowData in table.Rows)
            {
                var rowView = this.dataGridView.Rows[this.dataGridView.Rows.Add(1)];

                int column = 0;
                foreach (var cellData in rowData.Columns)
                {
                    var cellView = rowView.Cells[column];
                    cellView.Value = cellData;
                    column++;
                }
            }
        }

        private void OnSaveToFile(object sender, EventArgs e)
        {

        }

        private void OnCopyHashes(object sender, EventArgs e)
        {
            var hashes = new List<uint>();

            foreach (FileFormats.ResourceTypes.TableData table in this.comboBox.Items)
            {
                foreach (var hash in table.Columns.Select(c => c.NameHash))
                {
                    if (hashes.Contains(hash) == false)
                    {
                        hashes.Add(hash);
                    }
                }
            }

            hashes.Sort();

            var text = new StringBuilder();
            foreach (var hash in hashes)
            {
                text.AppendLine(hash.ToString("X8"));
            }

            Clipboard.SetText(text.ToString());
        }
    }
}
