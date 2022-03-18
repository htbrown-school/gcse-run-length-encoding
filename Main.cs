using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

class MainClass {
  public static string option;
  private static bool debug = false;

  public static string Input (string args) {
    Console.Write(args);
    string input = Console.ReadLine();
    return input;
  }

  public static string[] RleEncode(List<string> args) {
    string[] result = new string[args.Count];
    int count = 1;

    for (int i = 0; i < args.Count; i++) {
      for (int x = 0; x < args[i].Length; x++) {
        if (x + 1 < args[i].Length && args[i][x] == args[i][x + 1]) {
          count++;
        } else {
          result[i] += count.ToString("00");
          result[i] += args[i][x];
          count = 1;
        }
      }
    }

    return result;
  }

  public static string[] RleDecode(List<string> args) {
    string[] result = new string[args.Count];

    for (int i = 0; i < args.Count; i++) {
      for (int x = 0; x < args[i].Length; x++) {
        string line = args[i];
        if ((x + 1) % 3 == 0) {
          string amount = Convert.ToString(line[x - 2]) + Convert.ToString(line[x - 1]);
          result[i] += new String(line[x], Convert.ToInt32(amount));
        }
      }
    }
    return result;
  }

  public static void EnterRle() {
    int lines = Convert.ToInt32(Input("Enter Amount of Lines > "));

    while (lines <= 2) {
      Console.WriteLine("Invalid number of lines. Must be greater than 2.");
      lines = Convert.ToInt32(Input("Enter Amount of Lines > "));
    }

    string fileName = Input("Enter File Path > ");

    StreamWriter stream = new StreamWriter(fileName);
    List<string> list = new List<string>();

    using (stream) {
      for (int i = 0; i < lines; i++) {
        string line = Input("Enter Line " + Convert.ToString(i + 1) + " > ");
        list.Add(line);
        stream.WriteLine(line);
      }
    }

    Console.Clear();

    string[] decoded = RleDecode(list);
    for (int i = 0; i < decoded.Length; i++) {
      Console.WriteLine(decoded[i]);
    }

    Console.WriteLine();
    Console.WriteLine("Done! File saved to " + fileName + ". Press enter to return to menu.");
  }

  public static void DisplayAscii() {
    string fileName = Input("Enter File Path > ");
    StreamReader stream = new StreamReader(fileName);

    Console.Clear();
    Console.WriteLine("ASCII art contained in file " + fileName + " shown below. Press enter to return to menu.");
    Console.WriteLine();

    using (stream) {
      string line;

      while ((line = stream.ReadLine()) != null) {
        Console.WriteLine(line);
      }
    }
  }

  public static void ConvertRle() {
    string fileName = Input("Enter File Path > ");
    StreamReader streamr = new StreamReader(fileName);

    List<string> list = new List<string>();
    string line;

    int originalCount = 0;

    using (streamr) {
      while ((line = streamr.ReadLine()) != null) {
        list.Add(line);
        originalCount += line.Length;
      }
    }

    string[] rleList = RleEncode(list);
    int compressCount = 0;
    StreamWriter stream = new StreamWriter("result.txt");

    using (stream) {
      for (int i = 0; i < rleList.Length; i++) {
        stream.WriteLine(rleList[i]);
        compressCount += rleList[i].Length;
      }
    }

    Console.Clear();
    Console.WriteLine("Done! File saved to result.txt. Press enter to return to menu.");
    Console.WriteLine();
    Console.WriteLine("Uncompressed character count: " + Convert.ToString(originalCount));
    Console.WriteLine("Compressed character count: " + Convert.ToString(compressCount));
    Console.WriteLine("Saving: " + Convert.ToString(originalCount - compressCount));
  }

  public static void ConvertAscii() {
    string fileName = Input("Enter File Path > ");
    StreamReader streamr = new StreamReader(fileName);

    List<string> list = new List<string>();
    string line;

    using (streamr) {
      while ((line = streamr.ReadLine()) != null) {
        list.Add(line);
      }
    }

    try {
      string[] decoded = RleDecode(list);
      for (int i = 0; i < decoded.Length; i++) {
        Console.WriteLine(decoded[i]);
      }
    } catch (Exception e) {
      Console.WriteLine("Error: Invalid formatting. Maybe the file given was incorrect?");
      if (debug) {
        Console.WriteLine("[debug info follows]");
        Console.WriteLine(e);
      }
    }
  }

  public static void Main (string[] args) {
    while (true) {
      Console.Clear();

      Console.WriteLine("Choose an option:");
      Console.WriteLine();
      Console.WriteLine("1. Enter RLE");
      Console.WriteLine("2. Display ASCII Art");
      Console.WriteLine("3. Convert to ASCII Art");
      Console.WriteLine("4. Convert to RLE");
      Console.WriteLine("5. Quit");
      option = Input("> ");

      Console.Clear();
      
      switch (option) {
        case "1":
          EnterRle();
          Console.ReadLine();
          break;
        case "2":
          DisplayAscii();
          Console.ReadLine();
          break;
        case "3":
          ConvertAscii();
          Console.ReadLine();
          break;
        case "4":
          ConvertRle();
          Console.ReadLine();
          break;
        case "5":
          Console.WriteLine("Goodbye.");
          System.Environment.Exit(1);
          break;
      }
    }
  }
}