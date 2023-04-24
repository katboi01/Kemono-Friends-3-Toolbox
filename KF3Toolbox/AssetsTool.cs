using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using static KF3Parse;

public class AssetTool
{
    public AssetsManager am;

    private void ChangeFileVersion(int platformId, string input, string output, bool silent)
    {
        am = new AssetsManager();
        am.LoadClassPackage(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), @"classdata.tpk"));

        //Load file
        string selectedFile = input;
        BundleFileInstance bundleInst = null;
        try
        {
            bundleInst = am.LoadBundleFile(selectedFile, false);
            //Decompress the file to memory
            bundleInst.file = DecompressToMemory(bundleInst);
        }
        catch
        {
            if (!silent) Console.WriteLine($"Error: {Path.GetFileName(selectedFile)} is not a valid bundle file");
            return;
        }

        AssetsFileInstance inst = am.LoadAssetsFileFromBundle(bundleInst, 0);
        am.LoadClassDatabaseFromPackage(inst.file.Metadata.UnityVersion);

        inst.file.Metadata.TargetPlatform = (uint)platformId; //5-pc //13-android //20-webgl

        //commit changes
        byte[] newAssetData;
        using (MemoryStream stream = new MemoryStream())
        {
            using (AssetsFileWriter writer = new AssetsFileWriter(stream))
            {
                inst.file.Write(writer, 0, new List<AssetsReplacer>() { });
                newAssetData = stream.ToArray();
            }
        }

        BundleReplacerFromMemory bunRepl = new BundleReplacerFromMemory(inst.name, null, true, newAssetData, -1);

        //write a modified file (temp)
        string tempFile = Path.GetTempFileName();
        using (var stream = File.OpenWrite(tempFile))
        using (var writer = new AssetsFileWriter(stream))
        {
            bundleInst.file.Write(writer, new List<BundleReplacer>() { bunRepl });
        }
        bundleInst.file.Close();

        //load the modified file for compression
        bundleInst = am.LoadBundleFile(tempFile);
        using (var stream = File.OpenWrite(output))
        using (var writer = new AssetsFileWriter(stream))
        {
            bundleInst.file.Pack(bundleInst.file.Reader, writer, AssetBundleCompressionType.LZ4);
        }
        bundleInst.file.Close();

        File.Delete(tempFile);
        if (!silent) Console.WriteLine("complete");
        am.UnloadAll(); //delete this if something breaks
    }


    public void ReadParameterAsset(string filePath)
    {
        var local = KF3Parse.Instance;
        am = new AssetsManager();
        am.LoadClassPackage(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), @"classdata.tpk"));

        //Load file
        string selectedFile = filePath;
        BundleFileInstance bundleInst = null;
        try
        {
            bundleInst = am.LoadBundleFile(selectedFile, false);
            //Decompress the file to memory
            bundleInst.file = DecompressToMemory(bundleInst);
        }
        catch
        {
            Console.WriteLine($"Error: {Path.GetFileName(selectedFile)} is not a valid bundle file");
            return;
        }

        AssetsFileInstance inst = am.LoadAssetsFileFromBundle(bundleInst, 0);
        am.LoadClassDatabaseFromPackage(inst.file.Metadata.UnityVersion);

        foreach (var inf in inst.file.AssetInfos)
        {
            AssetTypeValueField baseField = am.GetBaseField(inst, inf);
            var fileName = am.GetBaseField(inst, inf)["m_Name"].Value.AsString;
            var dump = DumpJsonAsset(baseField);
            var id = -1;
            bool friend1 = false, friend2 = false, photo2 = false;
            var split = fileName.Split("_");
            if (split.Length > 1) int.TryParse(split[1], out id);
            if(split.Length > 2)
            {
                if (split[2] == "1") friend1 = true;
                if (split[2] == "2") friend2 = true;
                if (split[2] == "2") photo2 = true;
            }
            var charaData = local.CharaDatas.FirstOrDefault(c => c.id == id);
            switch (split[0])
            {
                case "ParamAbility":
                    {
                        var ability = JsonConvert.DeserializeObject<ParamAbility>(dump);
                        local.ParamAbilities.Add(ability);
                        if (charaData != null)
                        {
                            if (friend1)
                                charaData.ParamAbility1 = ability;
                            else if (friend2)
                                charaData.ParamAbility2 = ability;
                            else
                                charaData.ParamAbility = ability;
                        }
                        else
                        {
                            var photoData = local.PhotoDatas.FirstOrDefault(c => c.id == id);
                            if(photoData != null)
                            {
                                if (photo2)
                                    photoData.postEffect = ability;
                                else
                                    photoData.preEffect = ability;
                            }
                        }
                        break;
                    }
                case "ParamAlphaBase":
                    {
                        var alpha = JsonConvert.DeserializeObject<ParamAlphaBase>(dump);
                        local.ParamAlphaBases.Add(alpha);
                        if (charaData != null) charaData.ParamAlphaBase = alpha;
                        break;
                    }
                case "ParamArts":
                    {
                        var art = JsonConvert.DeserializeObject<ParamArts>(dump);
                        local.ParamArts1.Add(art);
                        if (charaData != null) charaData.ParamArts = art;
                        break;
                    }
                case "ParamSpecialAttack":
                    {
                        var spec = JsonConvert.DeserializeObject<ParamSpecialAttack>(dump);
                        local.ParamSpecialAttacks.Add(spec);
                        if (charaData != null) charaData.ParamSpecialAttack = spec;
                        break;
                    }
                case "ParamWaitAction":
                    {
                        var wait = JsonConvert.DeserializeObject<ParamWaitAction>(dump);
                        local.ParamWaits.Add(wait);
                        if (charaData != null) charaData.ParamWaitAction = wait;
                        break;
                    }

                case "ParamEnemyBase":
                    {
                        var enemyData = local.QuestEnemyDatas.FirstOrDefault(c => c.enemyCharaId == id);
                        var alpha = JsonConvert.DeserializeObject<ParamEnemyBase>(dump);
                        if (enemyData != null) enemyData.ParamEnemyBase = alpha;
                        break;
                    }
                default:
                    break;
            }
            //else Console.WriteLine(fileName);
        }

        bundleInst.file.Close();
        am.UnloadAll(); //delete this if something breaks
    }

    public static string DumpJsonAsset(AssetTypeValueField baseField)
    {
        JToken jBaseField = RecurseJsonDump(baseField, false);
        string json = JsonConvert.SerializeObject(JObject.Parse(jBaseField.ToString()));
        json = json.Replace("{\"Array\":[", "[").Replace("]},", "],");
        json = json.Remove(json.Length - 1, 1);
        if (!json.EndsWith("}")) json += "}";
        return json;
    }

    private static JToken RecurseJsonDump(AssetTypeValueField field, bool uabeFlavor)
    {
        AssetTypeTemplateField template = field.TemplateField;

        bool isArray = template.IsArray;

        if (isArray)
        {
            JArray jArray = new JArray();

            if (template.ValueType != AssetValueType.ByteArray)
            {
                for (int i = 0; i < field.Children.Count; i++)
                {
                    jArray.Add(RecurseJsonDump(field.Children[i], uabeFlavor));
                }
            }
            else
            {
                byte[] byteArrayData = field.Value.AsByteArray;
                for (int i = 0; i < byteArrayData.Length; i++)
                {
                    jArray.Add(byteArrayData[i]);
                }
            }

            return jArray;
        }
        else
        {
            if (field.Value != null)
            {
                AssetValueType evt = field.Value.ValueType;
                object value;
                switch (evt)
                {
                    case AssetValueType.Bool:
                        value = field.Value.AsBool; break;
                    case AssetValueType.Int8:
                    case AssetValueType.Int16:
                    case AssetValueType.Int32: value = field.Value.AsInt; break;
                    case AssetValueType.Int64: value = field.Value.AsLong; break;
                    case AssetValueType.UInt8:
                    case AssetValueType.UInt16:
                    case AssetValueType.UInt32: value = field.Value.AsUInt; break;
                    case AssetValueType.UInt64: value = field.Value.AsULong; break;
                    case AssetValueType.String: value = field.Value.AsString; break;
                    case AssetValueType.Float: value = field.Value.AsFloat; break;
                    case AssetValueType.Double: value = field.Value.AsDouble; break;
                    default: value = "invalid"; break;
                }

                return (JValue)JToken.FromObject(value);
            }
            else
            {
                JObject jObject = new JObject();

                for (int i = 0; i < field.Children.Count; i++)
                {
                    jObject.Add(field.Children[i].FieldName, RecurseJsonDump(field.Children[i], uabeFlavor));
                }

                return jObject;
            }
        }
    }

    private static AssetBundleFile DecompressToMemory(BundleFileInstance bundleInst)
    {
        AssetBundleFile bundle = bundleInst.file;

        MemoryStream bundleStream = new MemoryStream();
        bundle.Unpack(new AssetsFileWriter(bundleStream));

        bundleStream.Position = 0;

        AssetBundleFile newBundle = new AssetBundleFile();
        newBundle.Read(new AssetsFileReader(bundleStream));

        bundle.Reader.Close();
        return newBundle;
    }
}
