using Colossal;
using Colossal.IO.AssetDatabase;
using Colossal.Localization;
using Colossal.Logging;
using Colossal.PSI.Environment;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Game.UI.Localization;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using Game.PSI;

namespace EuropeanPortugueseLocale
{
    public class Mod : IMod
    {
        //public static ILog log = LogManager.GetLogger(nameof(EuropeanPortugueseLocale)).SetShowsErrorsInUI(true);
        public void OnLoad(UpdateSystem updateSystem)
        {
            GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset);
            CopyToModsData(Path.GetDirectoryName(asset.path));
        }

        public void CopyToModsData(string path)
        {

            string modsDataPath = Path.Combine(EnvPath.kUserDataPath, "ModsData", "EuropeanPortugueseLocale");
            string updatedFilePath = Path.Combine(modsDataPath, "updated.txt");
            string sourcePath = Path.Combine(path, ".Sources");
            string pathUpdatedFile = Path.Combine(sourcePath, "updated.txt");

            if (!Directory.Exists(modsDataPath))
            {
                Directory.CreateDirectory(modsDataPath);
            }

            if (File.Exists(updatedFilePath))
            {
                string modsDataUpdatedContent = File.ReadAllText(updatedFilePath);
                if (File.Exists(pathUpdatedFile))
                {
                    string pathUpdatedContent = File.ReadAllText(pathUpdatedFile);
                    if (modsDataUpdatedContent == pathUpdatedContent)
                    {
                        // No new update, do nothing.
                        return;
                    }
                }
            }

            if (Directory.Exists(sourcePath))
            {
                foreach (var file in Directory.GetFiles(sourcePath))
                {
                    string fileName = Path.GetFileName(file);
                    string destinationFile = Path.Combine(modsDataPath, fileName);

                    File.Copy(file, destinationFile, true);
                }
                NotificationSystem.Push("ptPT-restart-required",
                    title: "European Portugese Localization",
                    text: "É necessário reiniciar / Restart required",
                    onClicked: () => {
                        Application.Quit();
                });
            }
        }
        public void OnDispose()
        {
        }
    }
}
