//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. 
//

namespace wpfcore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnDrop(object sender, DragEventArgs dragEventArgs)
        {
            void enqueue(string n) => l.Items.Add(n);

            if (!(dragEventArgs.Data.GetData(DataFormats.FileDrop, autoConvert: false) is string[] names)) { return; }
            foreach (var name in names)
            {
                switch (name)
                {
                    case var d when d.IsDirectory():
                        Directory
                            .GetFiles(path: d, searchPattern: "*.mp3",
                                searchOption: SearchOption.AllDirectories)
                            .Where(f => f.IsDroppableFile())
                            .ForEach(enqueue);
                        break;
                    case var f when f.IsDroppableFile():
                        enqueue(f);
                        break;
                    default: 
                        break;
                }
            }
        }
    }

    public static class MyExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> ts, Action<T> action) { foreach (var t in ts) { action(t); } }

        public static bool IsDroppableFile(this string name) => File.Exists(name) && name.EndsWith(".mp3");

        public static bool IsDirectory(this string name) => Directory.Exists(name);
    }
}