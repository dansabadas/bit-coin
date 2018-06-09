using System;
using System.Collections.Generic;

namespace ConsoleApp1.PurelyFunctional
{
  public class ReactiveProgrammingFundamentals
  {
    public void Run()
    {
      void log(object messages)
      {
        if (messages == null)
        {
          Console.WriteLine("NULL");
          return;
        }

        if (typeof(List<dynamic>) == messages.GetType())
        {
          ((List<dynamic>)messages).ForEach(message => Console.WriteLine(message));
          return;
        }

        Console.WriteLine(messages);
      }

      //3.
      List<dynamic> projection()
      {
        var newReleases = new dynamic[]
        {
            new
            {
                id = 70111470,
                title = "Die Hard",
                boxart = "http://cdn-0.nflximg.com/images/2891/DieHard.jpg",
                uri = "http://api.netflix.com/catalog/titles/movies/70111470",
                rating = new double[] { 4.0 },
                bookmark = new dynamic[0]
            },
            new
            {
                id = 70111470,
                title = "Bad Boys",
                boxart = "http://cdn-0.nflximg.com/images/2891/BadBoys.jpg",
                uri = "http://api.netflix.com/catalog/titles/movies/70111470",
                rating = new double[] { 5.0 },
                bookmark = new dynamic[] { new { id = 432534, time = 65876586 } }
            }
        };
        var videoAndTitlePairs = new List<dynamic>();

        // ------------ INSERT CODE HERE! -----------------------------------
        // Use forEach function to accumulate {id, title} pairs from each video.
        // Put the results into the videoAndTitlePairs array 
        // ------------ INSERT CODE HERE! -----------------------------------
        new List<dynamic>(newReleases).ForEach(video => videoAndTitlePairs.Add(new { video.id, video.title}));

        return videoAndTitlePairs;
      }

      log(projection());
    }
  }
}
