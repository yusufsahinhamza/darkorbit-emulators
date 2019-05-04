package utils;


/**
 Class used to log everything happening on server
 */
public class Log {

    public static final String LINE_EQ    = "======================================";
    public static final String LINE_MINUS = "<---------------------------------------------------------------->";

    public static final String ANSI_RESET      = "\u001B[0m";
    public static final String ANSI_BLACK      = "\u001B[30m";
    public static final String ANSI_RED        = "\u001B[31m";
    public static final String ANSI_GREEN      = "\u001B[32m";
    public static final String ANSI_YELLOW     = "\u001B[33m";
    public static final String ANSI_BLUE       = "\u001B[34m";
    public static final String ANSI_PURPLE     = "\u001B[35m";
    public static final String ANSI_CYAN       = "\u001B[36m";
    public static final String ANSI_WHITE      = "\u001B[37m";
    public static final String ANSI_BOLD       = "\u001B[1m";
    public static final String ANSI_BOLD_RESET = "\u001B[21m";
    public static final String ANSI_BLINK      = "\u001B[5m";

    /**
     Method used to print a message to console along with thread name
     */
    public synchronized static void pt(final String... pMessages) {
    	if(Settings.TEXTS_ENABLED) {
        for (String message : pMessages) {
            p("Thread: " + Thread.currentThread()
                           .getName(), " | message: ", message);
          }
    	}
    }

    /**
     Method used to print a message to console
     */
    public synchronized static void p(final String... pMessages) {
        for (String message : pMessages) {
            System.out.print(message);
        }
        System.out.println();
    }


    /**
     Method used to break line in console
     */
    public static void br() {
        System.out.println();
    }

}
