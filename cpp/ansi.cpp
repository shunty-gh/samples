#include <iostream>

// Colours on https://ss64.com/nt/syntax-ansi.html

#define BLACK               "\e[30m"
#define RED                 "\e[31m"
#define GREEN               "\e[32m"
#define YELLOW              "\e[33m"
#define BLUE                "\e[34m"
#define MAGENTA             "\e[35m"
#define CYAN                "\e[36m"
#define WHITE               "\e[37m"

#define BRIGHT_BLACK        "\e[90m"
#define BRIGHT_RED          "\e[91m"
#define BRIGHT_GREEN        "\e[92m"
#define BRIGHT_YELLOW       "\e[93m"
#define BRIGHT_BLUE         "\e[94m"
#define BRIGHT_MAGENTA      "\e[95m"
#define BRIGHT_CYAN         "\e[96m"
#define BRIGHT_WHITE        "\e[97m"

#define BK_BLACK            "\e[40m"
#define BK_RED              "\e[41m"
#define BK_GREEN            "\e[42m"
#define BK_YELLOW           "\e[43m"
#define BK_BLUE             "\e[44m"
#define BK_MAGENTA          "\e[45m"
#define BK_CYAN             "\e[46m"
#define BK_WHITE            "\e[47m"

#define BK_BRIGHT_BLACK     "\e[100m"
#define BK_BRIGHT_RED       "\e[101m"
#define BK_BRIGHT_GREEN     "\e[102m"
#define BK_BRIGHT_YELLOW    "\e[103m"
#define BK_BRIGHT_BLUE      "\e[104m"
#define BK_BRIGHT_MAGENTA   "\e[105m"
#define BK_BRIGHT_CYAN      "\e[106m"
#define BK_BRIGHT_WHITE     "\e[107m"

#define RESET               "\e[0m"
#define BOLD                "\e[1m"
#define UNDERLINE           "\e[4m"
#define NO_UNDERLINE        "\e[24m"
#define INVERSE             "\e[7m"
#define NON_INVERSE         "\e[27m"

#define COLOR_0      BLACK
#define COLOR_1      BLUE
#define COLOR_2      GREEN
#define COLOR_3      CYAN
#define COLOR_4      RED
#define COLOR_5      MAGENTA
#define COLOR_6      YELLOW
#define COLOR_7      WHITE
#define COLOR_8      BRIGHT_BLACK
#define COLOR_9      BTIGHT_BLUE
#define COLOR_A      BRIGHT_GREEN
#define COLOR_B      BRIGHT_CYAN
#define COLOR_C      BRIGHT_RED
#define COLOR_D      BRIGHT_MAGENTA
#define COLOR_E      BRIGHT_YELLOW
#define COLOR_F      BRIGHT_WHITE

using std::cout; using std::endl;

int main()
{
    cout << CYAN "Some cyan colored text" << endl;
    cout << BK_RED "Add red background" RESET << endl;
    cout << "reset to default colors with RESET" << endl;

    printf(CYAN "Cyan using printf" RESET "\n");

    cout << MAGENTA "Purple text " << GREEN << "Followed by green" << RESET << endl;

    cout << COLOR_3 << "Cyan " << COLOR_F << BK_GREEN << "and bright white on green - you need to specify the background after the foreground" << RESET << endl;
    return EXIT_SUCCESS;
}
