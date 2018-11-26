using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace DesktopApp1
{
    public partial class Form1 : Form
    {
        //character stats
        static string charName;
        static string charClass;
        static int charlevel;
        static string charAlign;
        static int charEXP;
        static int charHPmax;
        static int charHPtemp;
        static int charHPcurrent;
        static bool inspiration;
        static int proficentBonus;
        static string charRace;
        static int STR;
        static int DEX;
        static int CON;
        static int INT;
        static int WIZ;
        static int CHA;
        static int copper;
        static int silver;
        static int gold;
        static string hitDie;
        static string speed;
        static string AC;
        static int deathFails;
        static int deathsucesses;

        // statMods
        static int strMod;
        static int dexMod;
        static int conMod;
        static int intMod;
        static int wizMod;
        static int chaMod;
        static int passiveWisdom;
        static string spellAbility;

        static int spellSaveDC;
        static int spellAttack;

        //skills

        // saving throws
        static bool strSavingthrow;
        static bool dexSavingthrow;
        static bool conSavingthrow;
        static bool intSavingthrow;
        static bool wisSavingthrow;
        static bool chaSavingthrow;

        static string filepath = @".\characters\sample.txt";
        void openFile()
        {
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory() + @"\characters";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filepath = openFileDialog1.FileName;
                getDataFromFile();
                setFormData();
            }
        }

        void saveAsFile()
        {
            saveFileDialog1.ShowDialog();
            getFormData();
            if (saveFileDialog1.FileName != "")
            {
                filepath = saveFileDialog1.FileName;
                setDataToFile();
                setFormData();
            }
        }

        // converts text in textboxes to a line for saving in character file
        string convertTextTofileLine(RichTextBox richTextBox)
        {
            StringBuilder Text = new StringBuilder(richTextBox.Text);
            Text.Replace('\n', '†');
            return Text.ToString();
        }

        // converts text in a string to a multiline input for textboxes
        string convertFileLineToMultiLine(string Text)
        {
            StringBuilder newText = new StringBuilder(Text);
            newText.Replace('†', '\n');
            return newText.ToString();
        }

        void getDataFromFile()
        {
            // read from chosen file (sample.txt by default)
            System.IO.StreamReader file = new System.IO.StreamReader(filepath);
            string line;
            List<string> charVars = new List<string>();
            while ((line = file.ReadLine()) != null)
            {
                System.Console.WriteLine(line);
                if (line[0] != ';')
                {
                    charVars.Add(line);
                }

            }
            file.Close();

            charName = charVars[0];
            charClass = charVars[1];
            charlevel = Int32.Parse(charVars[2]);
            charAlign = charVars[3];
            charEXP = Int32.Parse(charVars[4]);
            charHPmax = Int32.Parse(charVars[5]);
            charHPcurrent = Int32.Parse(charVars[6]);
            charHPtemp = Int32.Parse(charVars[7]);
            inspiration = bool.Parse(charVars[8]);
            charRace = charVars[9];
            STR = Int32.Parse(charVars[10]);
            DEX = Int32.Parse(charVars[11]);
            CON = Int32.Parse(charVars[12]);
            INT = Int32.Parse(charVars[13]);
            WIZ = Int32.Parse(charVars[14]);
            CHA = Int32.Parse(charVars[15]);
            gold = Int32.Parse(charVars[16]);
            silver = Int32.Parse(charVars[17]);
            copper = Int32.Parse(charVars[18]);
            spellAbility = charVars[19];
            hitDie = charVars[20];
            strSavingthrow = bool.Parse(charVars[21]);
            dexSavingthrow = bool.Parse(charVars[22]);
            conSavingthrow = bool.Parse(charVars[23]);
            intSavingthrow = bool.Parse(charVars[24]);
            wisSavingthrow = bool.Parse(charVars[25]);
            chaSavingthrow = bool.Parse(charVars[26]);
            speed = charVars[27];
            AC = charVars[28];
            deathFails = Int32.Parse(charVars[29]);
            deathsucesses = Int32.Parse(charVars[30]);
        }

        // updates dependant variables not stored in file
        void updateDependant()
        {
            if (charlevel <= 4)
            {
                proficentBonus = 2;
            }
            else if (charlevel <= 8)
            {
                proficentBonus = 3;
            }
            else if (charlevel <= 12)
            {
                proficentBonus = 4;
            }
            else if (charlevel <= 16)
            {
                proficentBonus = 5;
            }
            else if (charlevel <= 20)
            {
                proficentBonus = 6;
            }

            // this wierd math was required for negative modifiers to calculate correctly
            strMod = (int)Math.Floor((decimal)(STR - 10) / 2);
            dexMod = (int)Math.Floor((decimal)(DEX - 10) / 2);
            conMod = (int)Math.Floor((decimal)(CON - 10) / 2);
            intMod = (int)Math.Floor((decimal)(INT - 10) / 2);
            wizMod = (int)Math.Floor((decimal)(WIZ - 10) / 2);
            chaMod = (int)Math.Floor((decimal)(CHA - 10) / 2);

            passiveWisdom = 10 + wizMod;

            int spellMod = 0;
            if (spellAbility == "INT")
            {
                spellMod = intMod;
            }
            else if (spellAbility == "WIS")
            {
                spellMod = wizMod;
            }
            else
            {
                spellMod = chaMod;
            }

            spellSaveDC = 8 + proficentBonus + spellMod;
            spellAttack = proficentBonus + spellMod;
        }

        // turns an int to a string +/-int for display on form
        string signedIntToString(int integer)
        {
            return (integer <= 0) ? integer.ToString() : "+" + integer.ToString();
        }

        // calculates a skill/saving throw mod and converts to signedint
        string skillMod(bool prof, int mod)
        {
            int modifier = (prof) ? mod + proficentBonus : mod;
            return signedIntToString(modifier);
        }

        // sets forms data to character infomation (updates dependant vars)
        void setFormData()
        {
            updateDependant();
            CharacterName.Text = charName;
            CharacterClass.Text = charClass;
            CharacterLevel.Text = charlevel.ToString();
            characterAlignment.Text = charAlign;
            characterEXP.Text = charEXP.ToString();
            characterHPCurrent.Value = charHPcurrent;
            characterHPMax.Value = charHPmax;
            characterHPTemp.Value = charHPtemp;
            charInspiration.Checked = inspiration;
            profBonus.Text = signedIntToString(proficentBonus);
            characterRace.Text = charRace;
            strStat.Text = STR.ToString();
            dexStat.Text = DEX.ToString();
            conStat.Text = CON.ToString();
            intStat.Text = INT.ToString();
            wisStat.Text = WIZ.ToString();
            chaStat.Text = CHA.ToString();
            goldCount.Value = gold;
            silverCount.Value = silver;
            copperCount.Value = copper;
            HitDice.Text = hitDie;
            charSpeed.Text = speed;
            characterAC.Text = AC;
            DeathSaveFails.Value = deathFails;
            deathSuccess.Value = deathsucesses;


            StrModifier.Text = signedIntToString(strMod);
            DexModifier.Text = signedIntToString(dexMod);
            ConModifier.Text = signedIntToString(conMod);
            IntModifier.Text = signedIntToString(intMod);
            WizModifier.Text = signedIntToString(wizMod);
            ChaModifier.Text = signedIntToString(chaMod);
            

            if (spellAbility == "INT")
            {
                CastingInt.Checked = true;
            }
            else if (spellAbility == "WIS")
            {
                CastingWis.Checked = true;
            }
            else
            {
                CastingCha.Checked = true;
            }

            SpellAttack.Text = signedIntToString(spellAttack);
            spellSave.Text = signedIntToString(spellSaveDC);
            
            strSavingProf.Checked = strSavingthrow;
            dexSavingProf.Checked = dexSavingthrow;
            conSavingProf.Checked = conSavingthrow;
            intSavingProf.Checked = intSavingthrow;
            wisSavingProf.Checked = wisSavingthrow;
            chaSavingProf.Checked = chaSavingthrow;

            STRSaving.Text = skillMod(strSavingthrow, strMod);
            DEXSaving.Text = skillMod(dexSavingthrow, dexMod);
            CONSaving.Text = skillMod(conSavingthrow, conMod);
            INTSaving.Text = skillMod(intSavingthrow, intMod);
            WISSaving.Text = skillMod(wisSavingthrow, wizMod);
            CHASaving.Text = skillMod(chaSavingthrow, chaMod);
            PassiveWiz.Text = signedIntToString(passiveWisdom);
            //PassiveWiz.Text = skillMod(perceptionMod,10);
            CharIniative.Text = signedIntToString(dexMod);
        }

        // gets the form data and sets the character infomation to form data
        void getFormData()
        {
            charName = CharacterName.Text;
            charClass = CharacterClass.Text;
            charlevel = Int32.Parse(CharacterLevel.Text);
            charAlign = characterAlignment.Text;
            charEXP = Int32.Parse(characterEXP.Text);
            charHPcurrent = (int)characterHPCurrent.Value;
            charHPmax = (int)characterHPMax.Value;
            charHPtemp = (int)characterHPTemp.Value;
            inspiration = charInspiration.Checked;
            charRace = characterRace.Text;
            STR = Int32.Parse(strStat.Text);
            DEX = Int32.Parse(dexStat.Text);
            CON = Int32.Parse(conStat.Text);
            INT = Int32.Parse(intStat.Text);
            WIZ = Int32.Parse(wisStat.Text);
            CHA = Int32.Parse(chaStat.Text);
            gold = (int)goldCount.Value;
            silver = (int)silverCount.Value;
            copper = (int)copperCount.Value;
            hitDie = HitDice.Text;
            speed = charSpeed.Text;
            AC = characterAC.Text;
            deathFails = (int)DeathSaveFails.Value;
            deathsucesses = (int)deathSuccess.Value;

            if (CastingInt.Checked == true)
            {
                spellAbility = "INT";
            }
            else if (CastingWis.Checked == true)
            {
                spellAbility = "WIS";
            }
            else
            {
                spellAbility = "CHA";
            }

            strSavingthrow = strSavingProf.Checked;
            dexSavingthrow = dexSavingProf.Checked;
            conSavingthrow = conSavingProf.Checked;
            intSavingthrow = intSavingProf.Checked;
            wisSavingthrow = wisSavingProf.Checked;
            chaSavingthrow = chaSavingProf.Checked;

        }

        // sets the file to the data
        void setDataToFile()
        {
            // empty file and add lines
            File.WriteAllText(filepath, String.Empty);
            
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
            {
                file.WriteLine(charName);
                file.WriteLine(charClass);
                file.WriteLine(charlevel);
                file.WriteLine(charAlign);
                file.WriteLine(charEXP);
                file.WriteLine(charHPcurrent);
                file.WriteLine(charHPmax);
                file.WriteLine(charHPtemp);
                file.WriteLine(inspiration);
                file.WriteLine(charRace);
                file.WriteLine(STR);
                file.WriteLine(DEX);
                file.WriteLine(CON);
                file.WriteLine(INT);
                file.WriteLine(WIZ);
                file.WriteLine(CHA);
                file.WriteLine(gold);
                file.WriteLine(silver);
                file.WriteLine(copper);
                file.WriteLine(spellAbility);
                file.WriteLine(hitDie);
                file.WriteLine(strSavingthrow);
                file.WriteLine(dexSavingthrow);
                file.WriteLine(conSavingthrow);
                file.WriteLine(intSavingthrow);
                file.WriteLine(wisSavingthrow);
                file.WriteLine(chaSavingthrow);
                file.WriteLine(speed);
                file.WriteLine(AC);
                file.WriteLine(deathFails);
                file.WriteLine(deathsucesses);
            }

        }

        public Form1()
        {
            InitializeComponent();
            getDataFromFile();
            setFormData();
            // navigates spell site to the correct class if class is a spell caster
            string allSpellClass = "bard cleric druid paladin ranger sorcerer warlock wizard";
            if (allSpellClass.Contains(charClass.ToLower()))
            {
                string classUrl = "tags/" + charClass.ToLower() + ".html";
                webBrowser1.Navigate("https://thebombzen.com/grimoire/" + classUrl);
            }

        }

        // onValue update form function
        private void update(object sender, EventArgs e)
        {
            getFormData();
            setFormData();
        }

        // update file on closing
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Console.WriteLine("closing Form");
            // update variables
            getFormData();

            //save to file
            setDataToFile();
        }

        // manual save function (updates other fields)
        private void saveButton_Click(object sender, EventArgs e)
        {
            System.Console.WriteLine("saving character");
            // save
            getFormData();
            setDataToFile();
            // update
            getDataFromFile();
            setFormData();
        }

        /// <summary>
        ///  Money section:
        ///  gold, silver, copper, mechanics and transaction section 
        /// </summary>

        private void silverCount_ValueChanged(object sender, EventArgs e)
        {
            if (silverCount.Value == -1)
            {
                if (goldCount.Value > 0)
                {
                    silverCount.Value = 99;
                    goldCount.Value--;
                }
                else
                {
                    silverCount.Value = 0;
                }
            }
            else if (silverCount.Value >= 100)
            {
                goldCount.Value += (int)(silverCount.Value/100);
                silverCount.Value = (int)(silverCount.Value%100);
            }
        }

        private void copperCount_ValueChanged(object sender, EventArgs e)
        {
            if (copperCount.Value == -1)
            {
                if (silverCount.Value > 0)
                {
                    copperCount.Value = 9;
                    silverCount.Value--;
                }
                else
                {
                    copperCount.Value = 0;
                }
            }
            else if (copperCount.Value >= 10)
            {
                silverCount.Value += (int)(copperCount.Value/10);
                copperCount.Value = (int)(copperCount.Value%10);
            }
        }

        private void goldCount_ValueChanged(object sender, EventArgs e)
        {
            // placeholder
        }

        // add or subtract the values given from the "wallet"
        private void moneyTransaction_Click(object sender, EventArgs e)
        {
            copperCount.Value += transCopper.Value;
            silverCount.Value += transSilver.Value;
            goldCount.Value += transGold.Value;
        }

        /// <summary>
        ///  Death Saving throws logic
        /// </summary>

        // death condition
        private void DeathSaveFails_ValueChanged(object sender, EventArgs e)
        {

            if (DeathSaveFails.Value == 3)
            {
                Form2 deathMessage = new Form2();
                deathMessage.Show();
                DeathSaveFails.Value = 0;
                deathSuccess.Value = 0;
            }

        }

        // stabilisation (3 success death saves)
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (deathSuccess.Value == 3)
            {
                DeathSaveFails.Value = 0;
                deathSuccess.Value = 0;
            }
        }

        /// <summary>
        ///  code for dice roll section of application below:
        /// </summary>

        // random var for all dice rolls
        Random rand = new Random();
        // code for basic rolls
        private void rolldn(int n)
        {
            int result = rand.Next(1, n+1);
            DiceBox.Text = result.ToString();
            diceRoll_results.AppendText(result.ToString() + " (d"+n.ToString()+")");
            diceRoll_results.AppendText(Environment.NewLine);
        }
        // code for custom roll wont bother
        /*void customUserRoll()
        {
            //StringBuilder roll = new StringBuilder(customRoll.Text);
            string roll = customRoll.Text;
            int modify = 0;
            if (roll.Contains("+"))
            {
                modify = 1;
            }
            else if (roll.Contains("-"))
            {
                modify = 2;
            }
            int die = 0, n = 0;
            if (int.TryParse(roll[0]+"",out die) && int.TryParse(r)
            {

            }

            int result = rand.Next(1, n + 1);


            if (modify != 0) // normal dice result, return result;
            {

            }
            else // with modifer
            {

            }
            

        }*/

        // basic roll buttons
        private void d4_Click(object sender, EventArgs e)
        {
            rolldn(4);
        }

        private void d6_Click(object sender, EventArgs e)
        {
            rolldn(6);
        }

        private void d8_Click(object sender, EventArgs e)
        {
            rolldn(8);
        }

        private void d10_Click(object sender, EventArgs e)
        {
            rolldn(10);
        }

        private void d12_Click(object sender, EventArgs e)
        {
            rolldn(12);
        }

        private void d20_Click(object sender, EventArgs e)
        {
            rolldn(20);
        }

        private void advant_Click(object sender, EventArgs e)
        {
            int result1 = rand.Next(1, 21);
            int result2 = rand.Next(1, 21);
            if (result1 > result2)
            {
                DiceBox.Text = result1.ToString();
                diceRoll_results.AppendText(result1.ToString() + " (d20 - adv) - Other roll:" + result2.ToString());
            }
            else
            {
                DiceBox.Text = result2.ToString();
                diceRoll_results.AppendText(result2.ToString() + " (d20 - adv) - Other roll:" + result1.ToString());
            }
            diceRoll_results.AppendText(Environment.NewLine);
        }

        private void disvant_Click(object sender, EventArgs e)
        {
            int result1 = rand.Next(1, 21);
            int result2 = rand.Next(1, 21);
            if (result1 > result2)
            {
                DiceBox.Text = result2.ToString();
                diceRoll_results.AppendText(result2.ToString() + " (d20 - disadv)  - Other roll:" + result1.ToString());
            }
            else
            {
                DiceBox.Text = result1.ToString();
                diceRoll_results.AppendText(result1.ToString() + " (d20 - disadv)  - Other roll:" + result2.ToString());
            }
            diceRoll_results.AppendText(Environment.NewLine);
        }

        private void resetResults_Click(object sender, EventArgs e)
        {
            diceRoll_results.Text = "";
        }

        static bool a = false;
        private void button1_Click(object sender, EventArgs e)
        {
            if (a) {a = false;}
            else   {a = true;}
        }

        // Rolls ++ code
        private void nd4_Click(object sender, EventArgs e)
        {
            int randRoll = 0;
            int numDice = (int)d4inc.Value;
            for (int i = 0; i < numDice; i++)
            {
              randRoll += rand.Next(1, 5);
            }
            
            int rollMod = (int)rollModifier.Value;
            int result = randRoll + rollMod;
            string output = "(" + numDice.ToString() + "d4+" + rollMod.ToString() + ")";
            DiceBox.Text = output +" =" + result.ToString();

            diceRoll_results.AppendText(result.ToString()+output);
            diceRoll_results.AppendText(Environment.NewLine);
        }

        private void nd6_Click(object sender, EventArgs e)
        {
            int randRoll = 0;
            int numDice = (int)d6inc.Value;
            for (int i = 0; i < numDice; i++)
            {
                randRoll += rand.Next(1, 7);
            }

            int rollMod = (int)rollModifier.Value;
            int result = randRoll + rollMod;
            string output = "(" + numDice.ToString() + "d6+" + rollMod.ToString() + ")";
            DiceBox.Text = output + " =" + result.ToString();

            diceRoll_results.AppendText(result.ToString() + output);
            diceRoll_results.AppendText(Environment.NewLine);
        }

        private void nd8_Click(object sender, EventArgs e)
        {
            int randRoll = 0;
            int numDice = (int)d8inc.Value;
            for (int i = 0; i < numDice; i++)
            {
                randRoll += rand.Next(1, 9);
            }

            int rollMod = (int)rollModifier.Value;
            int result = randRoll + rollMod;
            string output = "(" + numDice.ToString() + "d8+" + rollMod.ToString() + ")";
            DiceBox.Text = output + " =" + result.ToString();

            diceRoll_results.AppendText(result.ToString() + output);
            diceRoll_results.AppendText(Environment.NewLine);
        }

        private void nd10_Click(object sender, EventArgs e)
        {
            int randRoll = 0;
            int numDice = (int)d10inc.Value;
            for (int i = 0; i < numDice; i++)
            {
                randRoll += rand.Next(1, 11);
            }

            int rollMod = (int)rollModifier.Value;
            int result = randRoll + rollMod;
            string output = "(" + numDice.ToString() + "d10+" + rollMod.ToString() + ")";
            DiceBox.Text = output + " =" + result.ToString();

            diceRoll_results.AppendText(result.ToString() + output);
            diceRoll_results.AppendText(Environment.NewLine);
        }

        private void nd12_Click(object sender, EventArgs e)
        {
            int randRoll = 0;
            int numDice = (int)d12inc.Value;
            for (int i = 0; i < numDice; i++)
            {
                randRoll += rand.Next(1, 13);
            }

            int rollMod = (int)rollModifier.Value;
            int result = randRoll + rollMod;
            string output = "(" + numDice.ToString() + "d12+" + rollMod.ToString() + ")";
            DiceBox.Text = output + " =" + result.ToString();

            diceRoll_results.AppendText(result.ToString() + output);
            diceRoll_results.AppendText(Environment.NewLine);
        }

        private void nd20_Click(object sender, EventArgs e)
        {
            int randRoll = 0;
            int numDice = (int)d20inc.Value;
            for (int i = 0; i < numDice; i++)
            {
                randRoll += rand.Next(1, 21);
            }

            int rollMod = (int)rollModifier.Value;
            int result = randRoll + rollMod;
            string output = "(" + numDice.ToString() + "d20+" + rollMod.ToString() + ")";
            DiceBox.Text = output + " =" + result.ToString();

            diceRoll_results.AppendText(result.ToString() + output);
            diceRoll_results.AppendText(Environment.NewLine);
        }

        private void advantMod_Click(object sender, EventArgs e)
        {
            int result1a = rand.Next(1, 21);
            int result2a = rand.Next(1, 21);
            int rollMod = (int)rollModifier.Value;
            int result1 = result1a + rollMod;
            int result2 = result2a + rollMod;

            if (result1a > result2a)
            {
                DiceBox.Text = "(d20+" + rollMod.ToString() + ")" + " ="+result1.ToString();
                diceRoll_results.AppendText(result1.ToString() + " (d20+"+ rollMod.ToString()+":advant)-Other roll:" + result2.ToString());
            }
            else
            {
                DiceBox.Text = "(d20+" + rollMod.ToString() + ")" + " =" + result2.ToString();
                diceRoll_results.AppendText(result2.ToString() + " (d20+" + rollMod.ToString() + ":advant)-Other roll:" + result1.ToString());
            }
            diceRoll_results.AppendText(Environment.NewLine);
        }

        private void disvantMod_Click(object sender, EventArgs e)
        {
            int result1a = rand.Next(1, 21);
            int result2a = rand.Next(1, 21);
            int rollMod = (int)rollModifier.Value;
            int result1 = result1a + rollMod;
            int result2 = result2a + rollMod;

            if (result1a > result2a)
            {
                DiceBox.Text = "(d20+" + rollMod.ToString() + ")" + " =" + result2.ToString();
                diceRoll_results.AppendText(result2.ToString() + " (d20+" + rollMod.ToString() + ":disadvant)-Other roll:" + result1.ToString());
            }
            else
            {
                DiceBox.Text = "(d20+" + rollMod.ToString() + ")" + " =" + result1.ToString();
                diceRoll_results.AppendText(result1.ToString() + " (d20+" + rollMod.ToString() + ":disadvant)-Other roll:" + result2.ToString());
            }
            diceRoll_results.AppendText(Environment.NewLine);
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Load_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void SaveAs_Click(object sender, EventArgs e)
        {
            saveAsFile();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string test = convertTextTofileLine(WeaponsAndEquipment);
            System.Console.WriteLine(test);
            System.Console.WriteLine(convertFileLineToMultiLine(test));
        }

        
    }
}
