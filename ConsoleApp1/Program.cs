using System;

int wrong_arg()
{
    Console.WriteLine("Wrong argument");
    return (1);
}

double get_percent(double sum, double rate, int year, int month)
{
    double percent = sum * rate * DateTime.DaysInMonth(year, month);
    double koef;
    if (DateTime.IsLeapYear(year))
        koef = 100 * 366;
    else
        koef = 100 * 365;
    percent /= koef;
    return percent;
}

double get_monthly(double sum, double rate, int term)
{
    double i = rate / (12 * 100);
    double i_plus_one_n = Math.Pow(1 + i, term);
    double monthly = sum * i * i_plus_one_n / (i_plus_one_n - 1);
    return monthly;
}

double ft_sum_red(double sum, double rate, int term, int sel_M, double payment)
{
    double init_sum = sum;
    double monthly = get_monthly(sum, rate, term);
    int year = DateTime.Today.Year;
    int month = DateTime.Today.Month;
    double res = 0;
    for (int it = 0; it < term; it++)
    {
        double percent = get_percent(sum, rate, year, month);
        sum += percent;
        sum -= monthly;
        res += monthly;
        if (it == sel_M - 1)
        {
            sum -= payment;
            monthly = get_monthly(sum, rate, term - sel_M);
            res += payment;
        }
        month++;
        if (month == 13)
        {
            month = 1;
            year++;
        }
    }
    res += sum;
    return res - init_sum;
}

double get_months(double sum, double monthly, double rate)
{
    double i = rate / (12 * 100);
    double months = Math.Log(monthly / (monthly - i * sum), 1 + i);
    return months;
}

double ft_time_red(double sum, double rate, int term, int sel_M, double payment)
{
    double init_sum = sum;
    double monthly = get_monthly(sum, rate, term);
    int year = DateTime.Today.Year;
    int month = DateTime.Today.Month;
    double res = 0;
    for (int it = 0; it < term; it++)
    {
        double percent = get_percent(sum, rate, year, month);
        sum += percent;
        sum -= monthly;
        res += monthly;
        if (it == sel_M - 1)
        {
            sum -= payment;
            it = -1;
            term = (int)Math.Ceiling(get_months(sum, monthly, rate));
            res += payment;
            sel_M = -1;
        }
        month++;
        if (month == 13)
        {
            month = 1;
            year++;
        }
    }
    res += sum;
    return res - init_sum;
}

if (args.Length != 5)
{
    Console.WriteLine("Wrong number of arguments");
    return 1;
}

if (!double.TryParse(args[0], out double sum) || sum < 0)
    return (wrong_arg());
if (!double.TryParse(args[1], out double rate) || rate < 0)
    return (wrong_arg());
if (!int.TryParse(args[2], out int term) || term < 0)
    return (wrong_arg());
if (!int.TryParse(args[3], out int sel_M) || sel_M < 0)
    return (wrong_arg());
if (!double.TryParse(args[4], out double payment) || payment < 0)
    return (wrong_arg());

double sum_red_op = Math.Round(ft_sum_red(sum, rate, term, sel_M, payment), 2);
double time_red_op = Math.Round(ft_time_red(sum, rate, term, sel_M, payment), 2);
string res;
if (sum_red_op < time_red_op)
    res = "Уменьшение срока выгоднее уменьшения платежа";
else if (sum_red_op > time_red_op)
    res = "Уменьшение платежа выгоднее уменьшения срока";
else
   res = "Уменьшение платежа равно уменьшению срока";
Console.WriteLine(res);
Console.WriteLine($"Переплата при уменьшении платежа : {sum_red_op}р.");
Console.WriteLine($"Переплата при уменьшении срока : {time_red_op}р.");
if (sum_red_op != time_red_op)
    Console.WriteLine($"{res} на {Math.Round(Math.Abs(sum_red_op - time_red_op), 2)}р.");
return 0;