#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QColorDialog>
#include <QFile>
#include <QFileDialog>
#include <QInputDialog>
#include <QMainWindow>
#include <QMessageBox>
#include <string>
QT_BEGIN_NAMESPACE
namespace Ui {
    class MainWindow;
}
QT_END_NAMESPACE


///
/// \brief This MainWindow class provides a main window for the sprite editor application, which contains
///        all the necessary UI elements and managing user manipulation.
///
/// \authors Miguel Mendoza, Matt Rogers, Logan Hunter,
/// Amelia Smith, Yohan Kwak, Yamin Zhuang
///
/// Reviewed by Yohan Kwak, Yamin Zhuang
///
class MainWindow : public QMainWindow {
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

protected:
    ///
    /// \brief Enables the "Last Frame" button.
    ///
    void enableLastButton();

    ///
    /// \brief Disables the "Last Frame" button.
    ///
    void disableLastButton();

    ///
    /// \brief Enables the "Next Frame" button.
    ///
    void enableNextButton();

    ///
    /// \brief Disables the "Next Frame" button.
    ///
    void disableNextButton();

    ///
    /// \brief Enables the "Delete Frame" button.
    ///
    void enableDeleteButton();

    ///
    /// \brief Disables the "Delete Frame" button.
    ///
    void disableDeleteButton();


    ///
    /// \brief Enables size buttons.
    ///
    void enableAllSizeButtons();

    ///
    /// \brief Disables all size buttons.
    ///
    void disableAllSizeButtons();

    ///
    /// \brief Disables the first size button.
    ///
    void disableSizeButton1();

    ///
    /// \brief Disables the second size button.
    ///
    void disableSizeButton2();

    ///
    /// \brief Disables the third size button.
    ///
    void disableSizeButton3();

    ///
    /// \brief Disables the fourth size button.
    ///
    void disableSizeButton4();

    ///
    /// \brief Disables the "Pause" button.
    ///
    void disablePause();

    ///
    /// \brief enables the "Pause" button and disables the "Play" button.
    ///
    void enablePauseDisablePlay();

    ///
    /// \brief Enables the "Play" button after a set time.
    ///
    void enablePlayWithTimer();

    ///
    /// \brief Enables the "Play" button.
    ///
    void enablePlay();

    ///
    /// \brief Updates the displayed frame number.
    /// \param frameNum The new frame number to display.
    ///
    void updateFrameNumber(int frameNum);

    ///
    /// \brief Changes the color of the "Color Picker" button.
    /// \param = color The new color for the button.
    ///
    void changeColorButton(QString color);

private:
    Ui::MainWindow *m_ui; // User interface for MainWindow.
};
#endif // MAINWINDOW_H
