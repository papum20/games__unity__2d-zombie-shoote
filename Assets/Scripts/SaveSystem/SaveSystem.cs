using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{


    #region HighScore

    public static void SaveScore(int score)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/score.zs";
        FileStream stream = new FileStream(path, FileMode.Create);

        HighScoreData scoreData = new HighScoreData(score);

        formatter.Serialize(stream, scoreData);
        stream.Close();
    }


    public static HighScoreData LoadScore()
    {
        string path = Application.persistentDataPath + "/score.zs";

        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            HighScoreData scoreData = formatter.Deserialize(stream) as HighScoreData;
            stream.Close();

            return scoreData;
        }
        else
        {
            Debug.LogError("no score save found");
            HighScoreData defaultScore = new HighScoreData(0);
            return defaultScore;
        }

    }


    #endregion


    #region OPTIONS


    public static void SaveOptions(int grenade, int rw, int rh, int sens, float _touch)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/options.zs";
        FileStream stream = new FileStream(path, FileMode.Create);

        OptionsSave optionsData = new OptionsSave(grenade, rw, rh, sens, _touch);

        formatter.Serialize(stream, optionsData);
        stream.Close();
    }

    public static OptionsSave LoadOptions()
    {
        string path = Application.persistentDataPath + "/opitons.zs";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            OptionsSave optionsData = formatter.Deserialize(stream) as OptionsSave;
            stream.Close();

            return optionsData;
        }
        else
        {
            Debug.LogError("no options save found");
            OptionsSave optionsData = new OptionsSave(8, 1280, 720, 70, 0.5f);
            return optionsData;
        }
    }

        #endregion

    }
