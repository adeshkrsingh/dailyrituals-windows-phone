using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using System.Windows.Input;
using DailyRituals.Commands;
using System.Runtime.Serialization;
using System.ComponentModel;
using Windows.UI.Popups;

namespace DailyRituals.DataModel
{
  public class Ritual  : INotifyPropertyChanged
  {
    public int ID { get; set; }

    public int maxday { get; set; }
    public int compdate { get; set; }
    public int counterNoOfTask = 0;

    public string Name { get; set; }
    public string Description { get; set; }
    public ObservableCollection<DateTime> Dates { get; set; }

    public string lastmodified { get; set; }

    [IgnoreDataMember]
    public ICommand CompletedCommand { get; set; }

    public Ritual()
    {
      CompletedCommand = new CompletedButtonClick();
      Dates = new ObservableCollection<DateTime>();
    }

    public void AddDate()
    {
      Dates.Add(DateTime.Today);
      NotifyPropertyChanged("Dates");


      string time = DateTime.Now.ToString();
      // displays.Text = time;
      lastmodified = "last modified =" + time;
    }


    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

  }

  public class DataSource
  {
    private ObservableCollection<Ritual> _rituals;

    public int counterNoOfTask = 0;
    const string fileName = "rituals.json";

    public DataSource()
    {
      _rituals = new ObservableCollection<Ritual>();
    }

    public async Task<ObservableCollection<Ritual>> GetRituals()
    {
      await ensureDataLoaded();
      return _rituals;
    }

    private async Task ensureDataLoaded()
    {
      if (_rituals.Count == 0)
        await getRitualDataAsync();

      return;
    }

    private async Task getRitualDataAsync()
    {
      if (_rituals.Count != 0)
        return;

      var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<Ritual>));

      try
      {
        // Add a using System.IO;
        using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(fileName))
        {
          _rituals = (ObservableCollection<Ritual>)jsonSerializer.ReadObject(stream);
        }
      }
      catch
      {
        _rituals = new ObservableCollection<Ritual>();
      }
    }

    public async void AddRitual(string name, string description, int maxd)
    {
       
      var ritual = new Ritual();
      ritual.Name = name;
      ritual.Description = description;
      ritual.Dates = new ObservableCollection<DateTime>();
      ritual.maxday = maxd;
      ritual.compdate = 0;

      string time = DateTime.Now.ToString();
      // displays.Text = time;
      ritual.lastmodified = time;

      _rituals.Add(ritual);
      await saveRitualDataAsync();
    }
  
    private async Task saveRitualDataAsync()
    {
      var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<Ritual>));
      using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(fileName,
          CreationCollisionOption.ReplaceExisting))
      {
        jsonSerializer.WriteObject(stream, _rituals);
      }
    }

    public async void CompleteRitualToday(Ritual ritual)
    {
      int index = _rituals.IndexOf(ritual);
      _rituals[index].AddDate();
      _rituals[index].compdate++;

      MessageDialog msg = new MessageDialog("your index =  " + index);
      await msg.ShowAsync();
      
      await saveRitualDataAsync();
    }
      public async void delCompleted()
    {
        MessageDialog msg;
        int index = 0; int found = 0;
        int x = _rituals.Count;
        for (int a = 1; a < x; a++ )
        {
           if( _rituals[a].compdate == _rituals[a].maxday )
           { 
               for (int b = a ; b< x-1-found ; b++)
               {
                   _rituals[b] = _rituals[b + 1];
                   msg = new MessageDialog("deleted =>>> " + _rituals[b].Name);
                   await msg.ShowAsync();
               } found++;
           }
        }
     /*     while(found>0)
          {
           //   _rituals[x - 1 - found].Name = "0";
              _rituals[x - 1].Description = "0";
              found--;
             
          }
      */

          _rituals[x - 1].Description = "0";

       msg = new MessageDialog("task completed");
        await msg.ShowAsync();
        await saveRitualDataAsync();
    }

  }
}
