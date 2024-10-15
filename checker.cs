using System;

namespace paradigm_shift_csharp
{
    class Checker
    {
        static float tolerancePercentage = 0.05f;

        static float GetWarningTolerance(float upperLimit)
        {
            return upperLimit * tolerancePercentage;
        }

        static bool IsTooLow(float value, float min, string parameterName, out string message)
        {
            if (value < min)
            {
                message = $"{parameterName} is too low!";
                return true;
            }
            message = string.Empty;
            return false;
        }

        static bool IsTooHigh(float value, float max, string parameterName, out string message)
        {
            if (value > max)
            {
                message = $"{parameterName} is too high!";
                return true;
            }
            message = string.Empty;
            return false;
        }

        static bool IsInWarningRange(float value, float min, float max, string parameterName, out string message, bool warnForParameter)
        {
            if (!warnForParameter)
            {
                message = $"{parameterName} is within the normal range.";
                return false;
            }
            
            message = GetWarningMessage(value, min, max, parameterName);
            return message != $"{parameterName} is within the normal range.";
        }
        
        static string GetWarningMessage(float value, float min, float max, string parameterName)
        {
            if (IsOutOfRange(value, min, max))
            {
                return $"{parameterName} is out of range!";
            }
            
            return GetApproachingWarning(value, min, max, parameterName);
        }
        
        static bool IsOutOfRange(float value, float min, float max)
        {
            return value < min || value > max;
        }
        
        static string GetApproachingWarning(float value, float min, float max, string parameterName)
        {
            float warningLowerLimit = min + GetWarningTolerance(max);
            float warningUpperLimit = max - GetWarningTolerance(max);
            
            if (value < warningLowerLimit)
            {
                return $"{parameterName} warning: Approaching discharge!";
            }
            
            if (value > warningUpperLimit)
            {
                return $"{parameterName} warning: Approaching charge-peak!";
            }
            
            return $"{parameterName} is within the normal range.";
        }

        static bool IsParameterInRange(float value, float min, float max, string parameterName, out string message, bool warnForParameter = true)
        {
            if (IsTooLow(value, min, parameterName, out message) || IsTooHigh(value, max, parameterName, out message))
            {
                return false;
            }

            IsInWarningRange(value, min, max, parameterName, out message, warnForParameter);
            return true;
        }
        
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
