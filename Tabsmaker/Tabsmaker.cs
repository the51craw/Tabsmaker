using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tabsmaker
{

    public partial class Tabsmaker : Form
    {
        Boolean[] notes = new Boolean[22]; // Corresponds to drawn notes and hit-areas regardless of # or b.
        Boolean[] augmentedNotes = new Boolean[37]; // Corresponds to real notes including # and b.
        Int32 augmentation = 0;
        Int32 notesIndex;
        Int32 augmentedNotesIndex;
        Int32[] augmentationtranslation = new int[] { 0, 1, 3, 5, 7, 8, 10, 12, 13, 15, 17, 19, 20, 22, 24, 25, 27, 29, 31, 32, 34, 36 };
        // e  f  g  a  h  c  d   e   f   g   a   h   c   d   e   f   g   a   h   c   d   e
        String[] noteNames = new string[] { "E3", "F3", "F#3", "G3", "G#3", "A3", "A#3", "B3", "C4", "C#4", "D4", "D#4",
                                            "E4", "F4", "F#4", "G4", "G#4", "A4", "A#4", "B4", "C5", "C#5", "D5", "D#5",
                                            "E5", "F5", "F#5", "G5", "G#5", "A5", "A#5", "B5", "C6", "C#6", "D6", "D#6", "E6"};
        Rectangle[] rectangles = new Rectangle[22];
        List<Position> Positions = new List<Position>();
        Int32 downClickedPosition;
        //Int32[]

        public Tabsmaker()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            Int32 x = 76;
            Int32 y = 177;
            Boolean odd = false;
            for (Int32 i = 0; i < 22; i++)
            {
                rectangles[i] = new Rectangle(x, y, 39, 26);
                x += 40;
                if (odd)
                {
                    y -= 8;
                    odd = false;
                }
                else
                {
                    y -= 9;
                    odd = true;
                }
            }
        }

        private void btnUpNeck_Click(object sender, EventArgs e)
        {

        }

        private void btnDownNeck_Click(object sender, EventArgs e)
        {

        }

        private void pbSystem_MouseDown(object sender, MouseEventArgs e)
        {
            downClickedPosition = e.Y;
            notesIndex = CheckForHit(e.X, e.Y);
            if (notesIndex > -1)
            {
                // You may only add 4 fingers!
                if (!notes[notesIndex] && GetFingerCount() >= 4)
                {
                    MessageBox.Show(this, "You may only add 4 notes!", "Tabs maker: Error!");
                }
                else
                {
                    notes[notesIndex] = !notes[notesIndex];
                    DrawNote(notesIndex);
                }
            }
        }

        private void pbSystem_MouseUp(object sender, MouseEventArgs e)
        {
            if (downClickedPosition > e.Y + 8)
            {
                augmentation = +1;
            }
            else if (downClickedPosition < e.Y - 8)
            {
                augmentation = -1;
            }
            else
            {
                augmentation = 0;
            }
            augmentedNotesIndex = augmentationtranslation[notesIndex] + augmentation;
            // Do not allow b before lowest E nor # before highest E:
            if (augmentedNotesIndex < 0)
            {
                augmentedNotesIndex = 0;
            }
            else if (augmentedNotesIndex > 36)
            {
                augmentedNotesIndex = 36;
            }
            lblNote.Text = noteNames[augmentedNotesIndex];
            UpdateFingerPositions();
        }

        private Int32 GetFingerCount()
        {
            Int32 count = 0;
            for (Int32 i = 20; i > -1; i--)
            {
                if (notes[i])
                {
                    count++;
                }
            }
            return count;
        }

        private void UpdateFingerPositions()
        {
            Positions.Clear();
            Boolean first = true;
            //Int32 stringNumber = 0; // 0 = thinnest string!

            // Start from top since using one finger position inhibits
            // using the same string for more finger positions.
            // Iterate all notes:
            for (Int32 i = 35; i > -1; i--)
            {
                //if (augmentedNotes[i])
                {
                    // Find all possible positions for this note.
                    Int32 pos = 36;

                    while (pos > -1)
                    {
                        if (noteNames[pos].StartsWith(lblNote.Text.Remove(lblNote.Text.Length - 1, 1))) // Compare label to list disregarding octave number
                        {
                            // If this is the first note found, create lists of Position
                            // elemets to put the notes in:
                            if (first)
                            {
                                Positions.Add(new Position());
                            }

                            augmentedNotes[pos] = true;

                            if (pos >= 24)
                            {
                                Positions[Positions.Count - 1].FingerPositions[0].String = 0;
                                Positions[Positions.Count - 1].FingerPositions[0].Fret = pos - 24;
                            }
                            else if (pos >= 19)
                            {
                                Positions[Positions.Count - 1].FingerPositions[1].String = 1;
                                Positions[Positions.Count - 1].FingerPositions[1].Fret = pos - 19;
                            }
                            else if (pos >= 15)
                            {
                                Positions[Positions.Count - 1].FingerPositions[2].String = 2;
                                Positions[Positions.Count - 1].FingerPositions[2].Fret = pos - 15;
                            }
                            else if (pos >= 10)
                            {
                                Positions[Positions.Count - 1].FingerPositions[3].String = 3;
                                Positions[Positions.Count - 1].FingerPositions[3].Fret = pos - 10;
                            }
                            else if (pos >= 5)
                            {
                                Positions[Positions.Count - 1].FingerPositions[4].String = 4;
                                Positions[Positions.Count - 1].FingerPositions[4].Fret = pos - 5;
                            }
                            else
                            {
                                Positions[Positions.Count - 1].FingerPositions[5].String = 5;
                                Positions[Positions.Count - 1].FingerPositions[5].Fret = pos;
                            }

                        }
                        pos--;


                        //for (Int32 pos = 0; pos < 4 && Positions[Positions.Count - 1].FingerPositions[pos] != null; pos++)
                        //{

                        //}
                        //Positions[Positions.Count - 1];
                    }

                    if (first)
                    {
                        first = false;
                    }

                    // Find all positions for this note on the fretboard:
                    // create Positions objects for them and fill out first
                }
            }
        }

        private Int32 CheckForHit(Int32 x, Int32 y)
        {
            for (Int32 i = 0; i < 22; i++)
            {
                if (x >= rectangles[i].Left
                    && x <= rectangles[i].Right
                    && y >= rectangles[i].Top
                    && y <= rectangles[i].Bottom)
                {
                    return i;
                }
            }
            return -1;
        }

        private void DrawNotes()
        {
            for (Int32 i = 0; i < 22; i++)
            {
                DrawNote(i);
            }
        }

        private void DrawNote(Int32 note)
        {
            Graphics g = pbSystem.CreateGraphics();
            if (notes[note])
            {
                g.FillEllipse(Brushes.Green, rectangles[note].Left + 6, rectangles[note].Top + 4, 23, 14);
            }
            else
            {
                g.FillEllipse(Brushes.Black, rectangles[note].Left + 6, rectangles[note].Top + 4, 23, 14);
            }
        }
    }

    /* Class position holds one possible combination of 
     * finger positions for the selected notes. Make a list
     * of Position objects to select from.
     */
    public class Position
    {
        public FingerPosition[] FingerPositions { get; set; }

        public Position()
        {
            FingerPositions = new FingerPosition[7];
            for (Int32 i = 0; i < 7; i++)
            {
                FingerPositions[i] = new FingerPosition();
            }
        }
    }

    /* Class FingerPosition holds one possible finger position
     * for one single note. Use in class Position.
     */
    public class FingerPosition
    {
        public Int32 String { get; set; }
        public Int32 Fret { get; set; }

        public FingerPosition()
        {
            String = -1; // Indicates not in use!
            Fret = -1;   // Indicates not in use!
        }
    }

    /* Class GuitarString holds a list of note numbers for one string.
     * Make 6 GuitarString objects and use to find notes.
     */
    public class GuitarString
    {
        public List<Int32> Frets { get; set; }

        public GuitarString(Int32 stringNumber, Int32 note)
        {
            Int32 lowest = 0;
            switch (stringNumber)
            {
                case 0:
                    lowest = 14;
                    break;
                case 1:
                    lowest = 11;
                    break;
                case 2:
                    lowest = 9;
                    break;
                case 3:
                    lowest = 6;
                    break;
                case 4:
                    lowest = 3;
                    break;
                case 5:
                    lowest = 0;
                    break;
            }
        }
    }
}
