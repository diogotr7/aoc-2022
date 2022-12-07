using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AdventOfCode
{
    public class Day07 : ISolution
    {
        public string Part1(string data)
        {
            Folder root = ParseFileSystem(data);

            var allDirectories = root.GetAllChildFoldersRecursively();

            return allDirectories.Where(f => f.Size <= 100000).Sum(asd => asd.Size).ToString();
        }

        public string Part2(string data)
        {
            const int TOTAL_SPACE = 70000000;
            const int TARGET_FREE_SPACE = 30000000;

            Folder root = ParseFileSystem(data);
            var allDirectories = root.GetAllChildFoldersRecursively();

            var usedSpace = root.Size;
            var currentFreeSpace = TOTAL_SPACE - usedSpace;
            var deleteTarget = TARGET_FREE_SPACE - currentFreeSpace;

            var smallestDelete = allDirectories.Select(s => s.Size)
                                               .Where(a => a > deleteTarget)
                                               .Min();

            return smallestDelete.ToString();
        }

        public interface IFileSystemStructure
        {
            string Name { get; }
            int Size { get; }
        }

        public class Folder : IFileSystemStructure
        {
            public string Name { get; }
            public int Size => Children?.Sum(c => c.Size) ?? 0;
            public List<IFileSystemStructure> Children { get; }

            public Folder? Parent { get; }

            public Folder(string name, Folder? parent)
            {
                Name = name;
                Parent = parent;
                Children = new();
            }

            internal IEnumerable<Folder> GetAllChildFoldersRecursively()
            {
                var childFolders = Children.OfType<Folder>();

                foreach (var item in childFolders)
                    childFolders = childFolders.Concat(item.GetAllChildFoldersRecursively());

                return childFolders;
            }
        }

        public class File : IFileSystemStructure
        {
            public string Name { get; }
            public int Size { get; }

            public File(string name, int size)
            {
                Name = name;
                Size = size;
            }
        }

        private static Folder ParseFileSystem(string data)
        {
            var commandsWithOutput = data.Split(Environment.NewLine + "$")
                                         .Skip(1)
                                         .Select(s => s.Trim());

            Folder root = new("/", null);
            Folder currentFolder = root;
            foreach (var commandWithOutput in commandsWithOutput)
            {
                var parts = commandWithOutput.Split(Environment.NewLine, 2);
                var command = parts[0].Trim();

                if (command == "ls")
                {
                    var output = parts[1].Trim().Split(Environment.NewLine);
                    foreach (var line in output)
                    {
                        var outputLineParts = line.Split(' ');
                        if (int.TryParse(outputLineParts[0], out var fileSize))
                            currentFolder.Children.Add(new File(outputLineParts[1], fileSize));
                        else if (outputLineParts[0] == "dir")
                            currentFolder.Children.Add(new Folder(outputLineParts[1], currentFolder));
                    }
                }
                else if (command.StartsWith("cd "))
                {
                    var cdTarget = command[3..];
                    if (cdTarget == "..")
                        currentFolder = currentFolder.Parent!;
                    else
                        currentFolder = (currentFolder.Children.First(c => c.Name == cdTarget) as Folder)!;
                }
            }

            return root;
        }
    }
}
