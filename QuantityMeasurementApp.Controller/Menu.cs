using System;
using QuantityMeasurementAppEntity.DTOs;

namespace QuantityMeasurementApp.Controller
{
    public static class Menu
    {
        public static void RunAllDemonstrations(QuantityMeasurementController controller)
        {
            bool running = true;
            while (running)
            {
                ShowMainMenu();
                string choice = (Console.ReadLine() ?? string.Empty).Trim();

                switch (choice)
                {
                    case "1": RunLengthMenu(controller);      break;
                    case "2": RunWeightMenu(controller);      break;
                    case "3": RunVolumeMenu(controller);      break;
                    case "4": RunTemperatureMenu(controller); break;
                    case "5": ShowHistory(controller);        break;
                    case "0":
                        running = false;
                        ShowFooter();
                        break;
                    default:
                        Console.WriteLine("  Invalid choice. Please try again.");
                        break;
                }
            }
        }

        // ── Main menu ─────────────────────────────────────────────────────────

        private static void ShowMainMenu()
        {
            Console.WriteLine();
            Console.WriteLine(new string('=', 50));
            Console.WriteLine("     QUANTITY MEASUREMENT APPLICATION");
            Console.WriteLine(new string('=', 50));
            Console.WriteLine("  1. Length");
            Console.WriteLine("  2. Weight");
            Console.WriteLine("  3. Volume");
            Console.WriteLine("  4. Temperature");
            Console.WriteLine("  5. View operation history");
            Console.WriteLine("  0. Exit");
            Console.WriteLine(new string('-', 50));
            Console.Write("  Enter choice: ");
        }

        // ── Length menu ───────────────────────────────────────────────────────

        private static void RunLengthMenu(QuantityMeasurementController controller)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine();
                Console.WriteLine("----- Length Measurement -----");
                Console.WriteLine($"  Supported units: {GetUnits("LENGTH")}");
                Console.WriteLine("  1. Compare");
                Console.WriteLine("  2. Convert");
                Console.WriteLine("  3. Add");
                Console.WriteLine("  4. Subtract");
                Console.WriteLine("  0. Back to main menu");
                Console.Write("  Enter choice: ");
                string choice = (Console.ReadLine() ?? string.Empty).Trim();

                switch (choice)
                {
                    case "1":
                        QuantityDTO cq1 = ReadQuantity("  First quantity",  "LENGTH");
                        QuantityDTO cq2 = ReadQuantity("  Second quantity", "LENGTH");
                        Console.Write("  Result: ");
                        controller.PerformComparison(cq1, cq2);
                        break;
                    case "2":
                        QuantityDTO cvSrc = ReadQuantity("  Source quantity", "LENGTH");
                        QuantityDTO cvTgt = ReadTargetUnit("LENGTH");
                        Console.Write("  Result: ");
                        controller.PerformConversion(cvSrc, cvTgt);
                        break;
                    case "3":
                        QuantityDTO aa1  = ReadQuantity("  First quantity",  "LENGTH");
                        QuantityDTO aa2  = ReadQuantity("  Second quantity", "LENGTH");
                        QuantityDTO atgt = ReadTargetUnit("LENGTH");
                        Console.Write("  Result: ");
                        controller.PerformAddition(aa1, aa2, atgt);
                        break;
                    case "4":
                        QuantityDTO ss1  = ReadQuantity("  First quantity",  "LENGTH");
                        QuantityDTO ss2  = ReadQuantity("  Second quantity", "LENGTH");
                        QuantityDTO stgt = ReadTargetUnit("LENGTH");
                        Console.Write("  Result: ");
                        controller.PerformSubtraction(ss1, ss2, stgt);
                        break;
                    case "0": back = true; break;
                    default:  Console.WriteLine("  Invalid choice."); break;
                }
            }
        }

        // ── Weight menu ───────────────────────────────────────────────────────

        private static void RunWeightMenu(QuantityMeasurementController controller)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine();
                Console.WriteLine("----- Weight Measurement -----");
                Console.WriteLine($"  Supported units: {GetUnits("WEIGHT")}");
                Console.WriteLine("  1. Compare");
                Console.WriteLine("  2. Convert");
                Console.WriteLine("  3. Add");
                Console.WriteLine("  4. Subtract");
                Console.WriteLine("  5. Divide");
                Console.WriteLine("  0. Back to main menu");
                Console.Write("  Enter choice: ");
                string choice = (Console.ReadLine() ?? string.Empty).Trim();

                switch (choice)
                {
                    case "1":
                        QuantityDTO cq1 = ReadQuantity("  First quantity",  "WEIGHT");
                        QuantityDTO cq2 = ReadQuantity("  Second quantity", "WEIGHT");
                        Console.Write("  Result: ");
                        controller.PerformComparison(cq1, cq2);
                        break;
                    case "2":
                        QuantityDTO cvSrc = ReadQuantity("  Source quantity", "WEIGHT");
                        QuantityDTO cvTgt = ReadTargetUnit("WEIGHT");
                        Console.Write("  Result: ");
                        controller.PerformConversion(cvSrc, cvTgt);
                        break;
                    case "3":
                        QuantityDTO aa1  = ReadQuantity("  First quantity",  "WEIGHT");
                        QuantityDTO aa2  = ReadQuantity("  Second quantity", "WEIGHT");
                        QuantityDTO atgt = ReadTargetUnit("WEIGHT");
                        Console.Write("  Result: ");
                        controller.PerformAddition(aa1, aa2, atgt);
                        break;
                    case "4":
                        QuantityDTO ss1  = ReadQuantity("  First quantity",  "WEIGHT");
                        QuantityDTO ss2  = ReadQuantity("  Second quantity", "WEIGHT");
                        QuantityDTO stgt = ReadTargetUnit("WEIGHT");
                        Console.Write("  Result: ");
                        controller.PerformSubtraction(ss1, ss2, stgt);
                        break;
                    case "5":
                        QuantityDTO dd1 = ReadQuantity("  First quantity",  "WEIGHT");
                        QuantityDTO dd2 = ReadQuantity("  Second quantity", "WEIGHT");
                        controller.PerformDivision(dd1, dd2);
                        break;
                    case "0": back = true; break;
                    default:  Console.WriteLine("  Invalid choice."); break;
                }
            }
        }

        // ── Volume menu ───────────────────────────────────────────────────────

        private static void RunVolumeMenu(QuantityMeasurementController controller)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine();
                Console.WriteLine("----- Volume Measurement -----");
                Console.WriteLine($"  Supported units: {GetUnits("VOLUME")}");
                Console.WriteLine("  1. Compare");
                Console.WriteLine("  2. Convert");
                Console.WriteLine("  3. Add");
                Console.WriteLine("  4. Subtract");
                Console.WriteLine("  0. Back to main menu");
                Console.Write("  Enter choice: ");
                string choice = (Console.ReadLine() ?? string.Empty).Trim();

                switch (choice)
                {
                    case "1":
                        QuantityDTO cq1 = ReadQuantity("  First quantity",  "VOLUME");
                        QuantityDTO cq2 = ReadQuantity("  Second quantity", "VOLUME");
                        Console.Write("  Result: ");
                        controller.PerformComparison(cq1, cq2);
                        break;
                    case "2":
                        QuantityDTO cvSrc = ReadQuantity("  Source quantity", "VOLUME");
                        QuantityDTO cvTgt = ReadTargetUnit("VOLUME");
                        Console.Write("  Result: ");
                        controller.PerformConversion(cvSrc, cvTgt);
                        break;
                    case "3":
                        QuantityDTO aa1  = ReadQuantity("  First quantity",  "VOLUME");
                        QuantityDTO aa2  = ReadQuantity("  Second quantity", "VOLUME");
                        QuantityDTO atgt = ReadTargetUnit("VOLUME");
                        Console.Write("  Result: ");
                        controller.PerformAddition(aa1, aa2, atgt);
                        break;
                    case "4":
                        QuantityDTO ss1  = ReadQuantity("  First quantity",  "VOLUME");
                        QuantityDTO ss2  = ReadQuantity("  Second quantity", "VOLUME");
                        QuantityDTO stgt = ReadTargetUnit("VOLUME");
                        Console.Write("  Result: ");
                        controller.PerformSubtraction(ss1, ss2, stgt);
                        break;
                    case "0": back = true; break;
                    default:  Console.WriteLine("  Invalid choice."); break;
                }
            }
        }

        // ── Temperature menu ──────────────────────────────────────────────────

        private static void RunTemperatureMenu(QuantityMeasurementController controller)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine();
                Console.WriteLine("----- Temperature Measurement -----");
                Console.WriteLine($"  Supported units: {GetUnits("TEMPERATURE")}");
                Console.WriteLine("  Note: Only Compare and Convert are supported.");
                Console.WriteLine("  1. Compare");
                Console.WriteLine("  2. Convert");
                Console.WriteLine("  0. Back to main menu");
                Console.Write("  Enter choice: ");
                string choice = (Console.ReadLine() ?? string.Empty).Trim();

                switch (choice)
                {
                    case "1":
                        QuantityDTO cq1 = ReadQuantity("  First quantity",  "TEMPERATURE");
                        QuantityDTO cq2 = ReadQuantity("  Second quantity", "TEMPERATURE");
                        Console.Write("  Result: ");
                        controller.PerformComparison(cq1, cq2);
                        break;
                    case "2":
                        QuantityDTO cvSrc = ReadQuantity("  Source quantity", "TEMPERATURE");
                        QuantityDTO cvTgt = ReadTargetUnit("TEMPERATURE");
                        Console.Write("  Result: ");
                        controller.PerformConversion(cvSrc, cvTgt);
                        break;
                    case "0": back = true; break;
                    default:  Console.WriteLine("  Invalid choice."); break;
                }
            }
        }

        // ── History ───────────────────────────────────────────────────────────

        private static void ShowHistory(QuantityMeasurementController controller)
        {
            Console.WriteLine();
            Console.WriteLine("----- Operation History -----");
            controller.ShowHistory();
        }

        // ── Input helpers ─────────────────────────────────────────────────────

        // Keeps asking until the user enters a valid "value unit" pair
        private static QuantityDTO ReadQuantity(string prompt, string type)
        {
            while (true)
            {
                Console.Write($"{prompt} (e.g. 1.0 FEET) [{GetUnits(type)}]: ");
                string line = (Console.ReadLine() ?? string.Empty).Trim();
                string[] parts = line.Split(' ', 2,
                    StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 2 &&
                    double.TryParse(parts[0], out double value))
                {
                    return new QuantityDTO(value, parts[1].ToUpper(), type);
                }

                Console.WriteLine(
                    "  Invalid input. Please enter both a number and a unit, " +
                    "e.g.  1.0 FEET");
            }
        }

        // Keeps asking until the user enters a recognised unit name
        private static QuantityDTO ReadTargetUnit(string type)
        {
            while (true)
            {
                Console.Write($"  Target unit [{GetUnits(type)}]: ");
                string unit = (Console.ReadLine() ?? string.Empty).Trim().ToUpper();

                if (!string.IsNullOrEmpty(unit))
                    return new QuantityDTO(0, unit, type);

                Console.WriteLine("  Unit cannot be empty. Please try again.");
            }
        }

        private static string GetUnits(string type)
        {
            switch (type)
            {
                case "LENGTH":      return "FEET, INCHES, YARDS, CENTIMETERS";
                case "WEIGHT":      return "KILOGRAM, GRAM, POUND";
                case "VOLUME":      return "LITRE, MILLILITRE, GALLON";
                case "TEMPERATURE": return "CELSIUS, FAHRENHEIT";
                default:            return string.Empty;
            }
        }

        private static void ShowFooter()
        {
            Console.WriteLine();
            Console.WriteLine(new string('=', 50));
            Console.WriteLine("  Thank you for using Quantity Measurement App.");
            Console.WriteLine(new string('=', 50));
        }
    }
}