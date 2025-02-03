#include "mainwindow.h"
#include <QApplication>

///
/// \brief Main class that starts the application with main window shown.
///
int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    MainWindow w;
    w.show();
    return a.exec();
}
