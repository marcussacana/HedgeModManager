﻿using HedgeModManager.Properties;
using SS16;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HedgeModManager
{
    public partial class InstallForm : Form
    {
        public InstallForm()
        {
            InitializeComponent();
        }

        public static List<Entry> FindGames()
        {
            var entries = new List<Entry>();
            var paths = new List<string>() {
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                AppDomain.CurrentDomain.BaseDirectory
            };

            string vdfLocation = Path.Combine(Steam.SteamLocation, "steamapps\\libraryfolders.vdf");
            if (File.Exists(vdfLocation)) {
                LogFile.AddMessage($"Reading VDF: {vdfLocation}");
                Steam.VDFFile vdf = null;
                try {
                    vdf = Steam.VDFFile.ReadVDF(vdfLocation);
                } catch (Exception ex) {
                    MainForm.AddMessage("Exception thrown while reading Steam's VDF file.", ex,
                    $"VDFPath: {vdfLocation}");
                }

                // Default Common Path
                paths.Add(Path.Combine(Steam.SteamLocation, "steamapps\\common"));

                // Gets all the custom libraries
                foreach (var library in vdf.Array.Elements)
                    if (int.TryParse(library.Key, out int index))
                        paths.Add(Path.Combine(library.Value.Value, "steamapps\\common"));
            }
            
            foreach (string path in paths)
                if (Directory.Exists(path))
                {
                    string lwPath = Path.Combine(path, "Sonic Lost World\\slw.exe");
                    string lwPath2 = Path.Combine(path, "SonicLostWorld\\slw.exe");
                    string lwPath3 = Path.Combine(path, "slw.exe");

                    string gensPath = Path.Combine(path, "Sonic Generations\\SonicGenerations.exe");
                    string gensPath2 = Path.Combine(path, "SonicGenerations\\SonicGenerations.exe");
                    string gensPath3 = Path.Combine(path, "SonicGenerations.exe");

                    string forcesPath = Path.Combine(path, "SonicForces\\build\\main\\projects\\exec\\Sonic Forces.exe");
                    string forcesPath2 = Path.Combine(path, "Sonic Forces\\build\\main\\projects\\exec\\Sonic Forces.exe");
                    string forcesPath3 = Path.Combine(path, "build\\main\\projects\\exec\\Sonic Forces.exe");

                    if (CheckGameAndSupport(lwPath) && !ContainsPath(lwPath, entries))
                        entries.Add(new Entry() { GameName = "Sonic Lost World", Path = lwPath });
                    else if (CheckGameAndSupport(lwPath2) && !ContainsPath(lwPath2, entries))
                        entries.Add(new Entry() { GameName = "Sonic Lost World", Path = lwPath2 });
                    else if (CheckGameAndSupport(lwPath3) && !ContainsPath(lwPath3, entries))
                        entries.Add(new Entry() { GameName = "Sonic Lost World", Path = lwPath3 });


                    if (CheckGameAndSupport(gensPath) && !ContainsPath(gensPath, entries))
                        entries.Add(new Entry() { GameName = "Sonic Generations", Path = gensPath });
                    else if (CheckGameAndSupport(gensPath2) && !ContainsPath(gensPath2, entries))
                        entries.Add(new Entry() { GameName = "Sonic Generations", Path = gensPath2 });
                    else if (CheckGameAndSupport(gensPath3) && !ContainsPath(gensPath3, entries))
                        entries.Add(new Entry() { GameName = "Sonic Generations", Path = gensPath3 });
                    

                    if (CheckGameAndSupport(forcesPath) && !ContainsPath(forcesPath, entries))
                        entries.Add(new Entry() { GameName = "Sonic Forces", Path = forcesPath });
                    if (CheckGameAndSupport(forcesPath2) && !ContainsPath(forcesPath2, entries))
                        entries.Add(new Entry() { GameName = "Sonic Forces", Path = forcesPath2 });
                    if (CheckGameAndSupport(forcesPath3) && !ContainsPath(forcesPath3, entries))
                        entries.Add(new Entry() { GameName = "Sonic Forces", Path = forcesPath3 });
                }
            return entries;
        }

        private static bool ContainsPath(string forcesPath, List<Entry> entries) {
            return (from x in entries where x.Path == forcesPath select x).Count() != 0;
        }

        public static bool CheckGameAndSupport(string path)
        {
            return File.Exists(path);
        }

        private void InstallForm_Load(object sender, EventArgs e)
        {
            try {
                Steam.Init();
            } catch { }

            if (Steam.SteamLocation == null)
            {
                LogFile.AddMessage($"Steam is not Setup correctly, " +
                    $"Please report this issue to {Program.ProgramName}'s GitHub Page with {Program.ProgramName}.log\n" +
                    $"Please copy all files that came with {Program.ProgramName} into your Game folder. Exiting...");
            }

            var games = FindGames();
            foreach (var game in games)
            {
                var lvi = new ListViewItem();
                lvi.Tag = game;
                lvi.SubItems[0].Text = game.GameName;
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, game.Path));
                listView1.Items.Add(lvi);
            }
            Theme.ApplyDarkThemeToAll(this);
        }

        public class Entry
        {
            public string GameName, Path;
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var game = listView1.SelectedItems[0].Tag as Entry;
                if (game != null)
                {
                    if (InstallModLoader(Path.GetDirectoryName(game.Path), game.GameName))
                        Close();
                }
            }
        }

        /// <summary>
        /// Copy all the ModLoader files into a custom directory
        /// and creates a shortcut, After that it restarts the ModLoader in that custom directory
        /// </summary>
        /// <param name="path">Path to an Executable</param>
        /// <param name="gameName">The Name of the game</param>
        /// <returns></returns>
        public bool InstallModLoader(string path, string gameName)
        {
            path = path.Replace('/', '\\');
            if (MessageBox.Show("Install Hedge Mod Manager in \n" + path + "?", Resources.ApplicationTitle,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // HedgeModManager.exe, HedgeModManager.pdb, cpkredir.dll, cpkredir.ini, cpkredir.txt
                var files = new string[] { Program.ExecutableName,
#if DEBUG
                    Path.ChangeExtension(Program.ExecutableName, "pdb"),
#endif
                    "cpkredir.dll",
                    "cpkredir.txt" };

                foreach (string file in files)
                {
                    string filePath = Path.Combine(Program.StartDirectory, file);
                    if (File.Exists(filePath))
                    {
                        // Copies the current file into the custom filepath
                        File.Copy(filePath, Path.Combine(path, file), true);

                        // Don't delete this file as its in use, same thing should be done for the pdb
                        if (file == Program.ExecutableName)
                            continue;

                        // Trys o delete the old files
                        try { File.Delete(filePath); } catch { }
                    }
                    else
                    { // Missing file has been detected
                        MessageBox.Show("Could not find " + file, Program.ProgramName);
                    }
                }

                LogFile.AddMessage("Creating Shortcut for " + gameName);

                try
                {
                    // Creates a shortcut to the ModLoader
                    string shortcutPath = Path.Combine(Program.StartDirectory, $"HedgeModManager - {gameName}.lnk");
                    var wsh = new IWshRuntimeLibrary.WshShell();
                    var shortcut = wsh.CreateShortcut(shortcutPath) as IWshRuntimeLibrary.IWshShortcut;
                    shortcut.Description = $"HedgeModManager - {gameName}";
                    shortcut.TargetPath = Path.Combine(path, Program.ExecutableName);
                    shortcut.WorkingDirectory = path;
                    shortcut.Save();
                    LogFile.AddMessage("    Done.");
                }
                catch (Exception ex)
                {
                }

                // Starts the ModLoader
                var startInfo = new ProcessStartInfo()
                {
                    FileName = Path.Combine(path, Program.ExecutableName),
                    Verb = "runas" // Run as Admin
                };
                Process.Start(startInfo);

                try
                {
                    // Trys to delete HedgeModManager.exe
                    startInfo = new ProcessStartInfo()
                    {
                        Arguments = $"/c powershell start-sleep 2 & del \"{Application.ExecutablePath}\"",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true,
                        FileName = "cmd.exe"
                    };
                    Process.Start(startInfo);
                }
                catch { }
                return true;
            }
            return false;
        }
    }
}
