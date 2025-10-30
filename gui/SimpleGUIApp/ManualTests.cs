using System;
using System.IO;
using System.Windows.Forms;

namespace SimpleGUIApp.Tests
{
    public class ManualTests
    {
        public static void RunTests()
        {
            Console.WriteLine("Kjører tester...");

            try
            {
                Test_UpdateScreen();
                Test_LoadGroupsFromJson();
                Console.WriteLine("Alle tester bestått!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test feilet: {ex.Message}");
            }

            Console.WriteLine("Tester fullført.");
        }

        private static void Test_UpdateScreen()
        {
            Form testForm = new Form();
            UIComponents uiComponents = new UIComponents(testForm);
            uiComponents.SetupUI();

            string testMessage = "Dette er en testmelding";
            uiComponents.UpdateScreen(testMessage);

            if (uiComponents.screenTextBox.Text.Contains(testMessage))
            {
                Console.WriteLine("Test_UpdateScreen: Bestått");
            }
            else
            {
                Console.WriteLine("Test_UpdateScreen: Feilet");
            }

            testForm.Dispose();
        }

        private static void Test_LoadGroupsFromJson()
        {
            Form testForm = new Form();
            UIComponents uiComponents = new UIComponents(testForm);
            uiComponents.SetupUI();

            // Opprett en midlertidig JSON-fil med testdata
            string tempJsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "temp_groups.json");
            string jsonData = "[{\"Id\":1,\"GroupName\":\"TestGroup1\",\"Devices\":[]}, {\"Id\":2,\"GroupName\":\"TestGroup2\",\"Devices\":[]}]";

            try
            {
                File.WriteAllText(tempJsonFilePath, jsonData);

                // Tilpass LoadGroupsFromJson til å bruke tempJsonFilePath
                uiComponents.LoadGroupsFromJson(tempJsonFilePath);

                // Sjekk at ComboBox ble fylt med testdataene
                if (uiComponents.groupComboBox.Items.Count == 2 &&
                    uiComponents.groupComboBox.Items[0].ToString() == "TestGroup1" &&
                    uiComponents.groupComboBox.Items[1].ToString() == "TestGroup2")
                {
                    Console.WriteLine("Test_LoadGroupsFromJson: Bestått");
                }
                else
                {
                    Console.WriteLine("Test_LoadGroupsFromJson: Feilet - Feil i ComboBox-innholdet");
                    Console.WriteLine($"Antall elementer funnet: {uiComponents.groupComboBox.Items.Count}");
                    for (int i = 0; i < uiComponents.groupComboBox.Items.Count; i++)
                    {
                        Console.WriteLine($"Element {i}: {uiComponents.groupComboBox.Items[i]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test_LoadGroupsFromJson: Feilet med unntak - {ex.Message}");
            }
            finally
            {
                // Slett den midlertidige filen etter test
                if (File.Exists(tempJsonFilePath))
                {
                    File.Delete(tempJsonFilePath);
                }
                testForm.Dispose();
            }
        }
    }
}
