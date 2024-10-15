using System;

namespace paradigm_shift_csharp
{
    class Checker
    {
        static float tolerancePercentage = 0.05f; // 5% tolerance for warnings

        // Method to calculate the warning tolerance for a parameter
        static float GetWarningTolerance(float upperLimit)
        {
            return upperLimit * tolerancePercentage;
        }

        // Refactored method to check individual parameters, including warning logic
        static bool IsParameterInRange(float value, float min, float max, string parameterName, out string message, bool warnForParameter = true)
        {
            float warningLowerLimit = min + GetWarningTolerance(max);
            float warningUpperLimit = max - GetWarningTolerance(max);

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
            else if (warnForParameter && value >= min && value < warningLowerLimit)
            {
                message = $"{parameterName} warning: Approaching discharge!";
            }
            else if (warnForParameter && value > warningUpperLimit && value <= max)
            {
                message = $"{parameterName} warning: Approaching charge-peak!";
            }
            else
            {
                message = $"{parameterName} is within the normal range.";
            }
            return true;
        }

        // Function to check battery status and include warnings for relevant parameters
        static bool BatteryIsOk(float temperature, float soc, float chargeRate, bool warnForTemperature = true, bool warnForSoc = true, bool warnForChargeRate = true)
        {
            string message;

            bool temperatureOk = IsParameterInRange(temperature, 0, 45, "Temperature", out message, warnForTemperature);
            Console.WriteLine(message);

            bool socOk = IsParameterInRange(soc, 20, 80, "State of Charge", out message, warnForSoc);
            Console.WriteLine(message);

            bool chargeRateOk = IsParameterInRange(chargeRate, 0, 0.8f, "Charge Rate", out message, warnForChargeRate);
            Console.WriteLine(message);

            return temperatureOk && socOk && chargeRateOk;
        }

        // Test cases with warning checks
        static void RunTests()
        {
            ExpectTrue(BatteryIsOk(25, 70, 0.7f));   // Normal scenario
            ExpectFalse(BatteryIsOk(50, 70, 0.7f));  // High temperature
            ExpectFalse(BatteryIsOk(-5, 70, 0.7f));  // Low temperature
            ExpectFalse(BatteryIsOk(25, 85, 0.7f));  // High SOC
            ExpectFalse(BatteryIsOk(25, 15, 0.7f));  // Low SOC
            ExpectFalse(BatteryIsOk(25, 70, 0.9f));  // High charge rate
            ExpectTrue(BatteryIsOk(25, 21, 0.7f));   // Approaching discharge warning for SOC
            ExpectTrue(BatteryIsOk(25, 79, 0.7f));   // Approaching charge-peak warning for SOC
        }

        // Assertion functions
        static void ExpectTrue(bool expression)
        {
            if (!expression)
            {
                Console.WriteLine("Expected true, but got false");
                Environment.Exit(1);
            }
        }

        static void ExpectFalse(bool expression)
        {
            if (expression)
            {
                Console.WriteLine("Expected false, but got true");
                Environment.Exit(1);
            }
        }

        static int Main()
        {
            RunTests();
            Console.WriteLine("All tests passed");
            return 0;
        }
    }
}
