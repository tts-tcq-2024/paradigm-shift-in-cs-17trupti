using System;

namespace paradigm_shift_csharp
{
    class Checker
    {
        // Pure function to check individual parameters and return the status
        static bool IsParameterInRange(float value, float min, float max, string parameterName, out string message)
        {
            if (value < min)
            {
                message = $"{parameterName} is too low!";
                return false;
            }
            else if (value > max)
            {
                message = $"{parameterName} is too high!";
                return false;
            }
            message = $"{parameterName} is within the range.";
            return true;
        }

        // Function to check if battery is OK by checking all parameters
        static bool BatteryIsOk(float temperature, float soc, float chargeRate)
        {
            string message;
            bool temperatureOk = IsParameterInRange(temperature, 0, 45, "Temperature", out message);
            Console.WriteLine(message);

            bool socOk = IsParameterInRange(soc, 20, 80, "State of Charge", out message);
            Console.WriteLine(message);

            bool chargeRateOk = IsParameterInRange(chargeRate, 0, 0.8f, "Charge Rate", out message);
            Console.WriteLine(message);

            return temperatureOk && socOk && chargeRateOk;
        }

        // Test function to assert true conditions
        static void ExpectTrue(bool expression)
        {
            if (!expression)
            {
                Console.WriteLine("Expected true, but got false");
                Environment.Exit(1);
            }
        }

        // Test function to assert false conditions
        static void ExpectFalse(bool expression)
        {
            if (expression)
            {
                Console.WriteLine("Expected false, but got true");
                Environment.Exit(1);
            }
        }

        // Test function to cover different scenarios
        static void RunTests()
        {
            ExpectTrue(BatteryIsOk(25, 70, 0.7f));   // Normal scenario
            ExpectFalse(BatteryIsOk(50, 70, 0.7f));  // High temperature
            ExpectFalse(BatteryIsOk(-5, 70, 0.7f));  // Low temperature
            ExpectFalse(BatteryIsOk(25, 85, 0.7f));  // High SOC
            ExpectFalse(BatteryIsOk(25, 15, 0.7f));  // Low SOC
            ExpectFalse(BatteryIsOk(25, 70, 0.9f));  // High charge rate
        }

        static int Main()
        {
            RunTests();
            Console.WriteLine("All tests passed");
            return 0;
        }
    }
}
