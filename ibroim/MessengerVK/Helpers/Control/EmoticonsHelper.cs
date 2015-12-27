using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows;

namespace MessengerVK.Helpers.Control
{
  class EmoticonsHelper
  {
    static IDictionary<string, string> m_Emoticons = null;
    static IDictionary<string, string> m_EmoticonsReverse = null;

    private static void InitEmoticons()
    {
      if (m_Emoticons != null)
        return;

      GetEmotions();

        m_EmoticonsReverse = new Dictionary<string, string>();
      foreach (string k in m_Emoticons.Keys)
        m_EmoticonsReverse[m_Emoticons[k]] = k;
    }

      private static void GetEmotions()
      {
          m_Emoticons = new Dictionary<string, string>();

          m_Emoticons["^^"] = "../../Smileys/01.png";
          m_Emoticons["^_^"] = "../../Smileys/01.png";
          m_Emoticons[":D"] = "../../Smileys/02.png";
          m_Emoticons[":-D"] = "../../Smileys/03.png";
          m_Emoticons[";)"] = "../../Smileys/04.png";
          m_Emoticons[";-)"] = "../../Smileys/04.png";
          m_Emoticons[":)"] = "../../Smileys/05.png";
          m_Emoticons[":-)"] = "../../Smileys/05.png";
          m_Emoticons["8)"] = "../../Smileys/06.png";
          m_Emoticons["8-)"] = "../../Smileys/07.png";
          m_Emoticons[":p"] = "../../Smileys/08.png";
          m_Emoticons[":-p"] = "../../Smileys/08.png";
          m_Emoticons[":o"] = "../../Smileys/10.png";
          m_Emoticons[":-o"] = "../../Smileys/10.png";
          m_Emoticons[":("] = "../../Smileys/12.png";
          m_Emoticons[":-("] = "../../ Smileys/12.png";
          m_Emoticons[":'("] = "../../Smileys/13.png";
          m_Emoticons[":'-("] = "../../Smileys/13.png";
          m_Emoticons[":@"] = "../../Smileys/14.png";
          m_Emoticons[":-@"] = "../../Smileys/14.png";
          m_Emoticons[">:@"] = "../../Smileys/14.png";
          m_Emoticons[">:-@"] = "../../Smileys/14.png";
          m_Emoticons[">_<"] = "../../Smileys/14.png";
      }

// InitEmoticons

    public static string GetPlainText(FlowDocument doc)
    {
      InitEmoticons();
      StringBuilder result = new StringBuilder();

      foreach (Block b in doc.Blocks) {
        Paragraph p = b as Paragraph;
        if (p != null) {
          foreach (Inline i in p.Inlines){
            if (i is Run){
              Run r = i as Run;
              result.Append(r.Text);
            } else if (i is InlineUIContainer) {
              InlineUIContainer ic = i as InlineUIContainer;
              if (ic.Child is Image) {
                BitmapImage img = (ic.Child as Image).Source as BitmapImage;
                result.Append(m_EmoticonsReverse[img.UriSource.ToString()]);
              }
            }
          }
        }
      }
      return result.ToString();
    } // GetPlainText

    private static int FindFirstEmoticon(string text, int startIndex, out string emoticonFound)
    {
      InitEmoticons();
      emoticonFound = string.Empty;
      int minIndex = -1;
      foreach (string e in m_Emoticons.Keys) {
        int index = text.IndexOf(e, startIndex);
        if (index >= 0) {
          if (minIndex < 0 || index < minIndex) {
            minIndex = index;
            emoticonFound = e;
          }
        }
      }
      return minIndex;
    } // FindFirstEmoticon

    public static void ParseText(FrameworkElement element)
    {
      InitEmoticons();
      TextBlock textBlock = null;
      RichTextBox textBox = element as RichTextBox;
      if (textBox == null)
        textBlock = element as TextBlock;
      
      if (textBox == null && textBlock == null)
        return;

      if (textBox != null){
        FlowDocument doc = textBox.Document;
        for (int blockIndex=0; blockIndex < doc.Blocks.Count; blockIndex++){
          Block b = doc.Blocks.ElementAt(blockIndex);
          Paragraph p = b as Paragraph;
          if (p != null) {          
            ProcessInlines(textBox, p.Inlines);
          }
        }
      }else{
        ProcessInlines(null, textBlock.Inlines);
      }
    } // ParseText

    private static void ProcessInlines(RichTextBox textBox, InlineCollection inlines)
    {
      for (int inlineIndex=0; inlineIndex < inlines.Count; inlineIndex++){
        Inline i = inlines.ElementAt(inlineIndex);
        if (i is Run) {
          Run r = i as Run;
          string text = r.Text;
          string emoticonFound = string.Empty;
          int index = FindFirstEmoticon(text, 0, out emoticonFound);
          if (index >= 0) {
            TextPointer tp = i.ContentStart;
            bool reposition = false;                
            while (!tp.GetTextInRun(LogicalDirection.Forward).StartsWith(emoticonFound)) 
              tp = tp.GetNextInsertionPosition(LogicalDirection.Forward);
            TextPointer end = tp;
            for (int j=0; j<emoticonFound.Length; j++)
              end = end.GetNextInsertionPosition(LogicalDirection.Forward);
            TextRange tr = new TextRange(tp, end);
            if (textBox != null)
              reposition = textBox.CaretPosition.CompareTo(tr.End) == 0;
            tr.Text = string.Empty;

            string imageFile = m_Emoticons[emoticonFound];
            Image image = new Image();
            BitmapImage bimg = new BitmapImage();
            bimg.BeginInit();
            bimg.UriSource = new Uri(imageFile, UriKind.Relative);
            bimg.EndInit();
            image.Source = bimg;
            image.Width =24;

            InlineUIContainer iui = new InlineUIContainer(image, tp);
            iui.BaselineAlignment = BaselineAlignment.TextBottom;

            if (textBox != null && reposition)
              textBox.CaretPosition = tp.GetNextInsertionPosition(LogicalDirection.Forward);
          }
        }
      }
    } // ProcessInlines
  }
}
