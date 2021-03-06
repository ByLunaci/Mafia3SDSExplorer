﻿using System;
using System.IO;
using System.Windows.Forms;

namespace Gibbed.Illusion.ExploreSDS
{
    public partial class TextureViewer : Form
    {
        public FileFormats.SdsMemory.Entry Entry;
        public FileFormats.ResourceTypes.TextureResource Resource;

        public TextureViewer()
        {
            this.InitializeComponent();
        }

        public void LoadFile(FileFormats.SdsMemory.Entry entry)
        {
            var resource = new FileFormats.ResourceTypes.TextureResource();
            resource.Deserialize(entry.Header, entry.Data);
            this.Text += ": " + entry.Description;

            this.Entry = entry;
            this.Resource = resource;

            this.UpdatePreview();
        }

        private void OnSave(object sender, System.EventArgs e)
        {
            var data = new MemoryStream();
            this.Resource.Serialize(this.Entry.Header, data);
            this.Entry.Data = data;
        }

        private void UpdatePreview()
        {
            MemoryStream memory = new MemoryStream();
            memory.Write(this.Resource.Data, 0, this.Resource.Data.Length);
            memory.Position = 0L;
            Gibbed.Squish.DDSFile expr_32 = new Gibbed.Squish.DDSFile();
            expr_32.Deserialize(memory);
            var image = expr_32.Image(true, true, true, this.toggleAlphaButton.Checked);
            if (this.toolStripButton1.Checked)
            {
                this.previewPictureBox.Dock = DockStyle.Fill;
                this.previewPictureBox.Image = image;
                this.previewPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                return;
            }
            this.previewPictureBox.Dock = DockStyle.None;
            this.previewPictureBox.Image = image;
            this.previewPictureBox.Width = image.Width;
            this.previewPictureBox.Height = image.Height;
            this.previewPictureBox.SizeMode = PictureBoxSizeMode.Normal;
        }

        private void OnZoom(object sender, EventArgs e)
        {
            this.UpdatePreview();
        }

        private void OnToggleAlpha(object sender, EventArgs e)
        {
            this.UpdatePreview();
        }

        private void OnSaveToFile(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(this.saveFileDialog.FileName) == true)
            {
                this.saveFileDialog.FileName = this.Entry.Description;
            }

            if (this.saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            using (var output = File.Open(this.saveFileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                output.Write(this.Resource.Data, 4, this.Resource.Data.Length);
            }
        }

        private void OnLoadFromFile(object sender, System.EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            using (var input = File.Open(
                this.openFileDialog.FileName,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite))
            {
                this.Resource.Data = new byte[input.Length];
                input.Read(this.Resource.Data, 0, this.Resource.Data.Length);
                this.UpdatePreview();
            }
        }
    }
}
