/********************************************************************************
** Form generated from reading UI file 'mainwindow.ui'
**
** Created by: Qt User Interface Compiler version 6.4.3
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_MAINWINDOW_H
#define UI_MAINWINDOW_H

#include <QtCore/QVariant>
#include <QtWidgets/QApplication>
#include <QtWidgets/QComboBox>
#include <QtWidgets/QLabel>
#include <QtWidgets/QMainWindow>
#include <QtWidgets/QMenuBar>
#include <QtWidgets/QPushButton>
#include <QtWidgets/QStatusBar>
#include <QtWidgets/QWidget>
#include "canvas.h"
#include "preview.h"

QT_BEGIN_NAMESPACE

class Ui_MainWindow
{
public:
    QWidget *centralwidget;
    QPushButton *brush1Btn;
    QPushButton *brush4Btn;
    QPushButton *brush2Btn;
    QPushButton *brush3Btn;
    QPushButton *tileButton;
    QPushButton *fillButton;
    QComboBox *fpsCombo;
    QLabel *fpsLabel;
    QPushButton *saveButton;
    QPushButton *loadButton;
    QPushButton *setSpriteSizeButton;
    Canvas *canvasWidget;
    QPushButton *zoomInButton;
    QPushButton *zoomOutButton;
    QLabel *currentFrame;
    QPushButton *lastFrame;
    QPushButton *nextFrame;
    QPushButton *deleteCurrentFrame;
    QPushButton *addFrame;
    Preview *previewWidget;
    QPushButton *playButton;
    QPushButton *pauseButton;
    QLabel *previewLabel;
    QLabel *brushEraserSize;
    QPushButton *smallPreviewButton;
    QPushButton *brushButton;
    QPushButton *colorPickBtn;
    QLabel *colorSelector;
    QPushButton *eraserButton;
    QPushButton *duplicateFrame;
    QPushButton *clearFrame;
    QMenuBar *menubar;
    QStatusBar *statusbar;

    void setupUi(QMainWindow *MainWindow)
    {
        if (MainWindow->objectName().isEmpty())
            MainWindow->setObjectName("MainWindow");
        MainWindow->resize(1298, 676);
        centralwidget = new QWidget(MainWindow);
        centralwidget->setObjectName("centralwidget");
        brush1Btn = new QPushButton(centralwidget);
        brush1Btn->setObjectName("brush1Btn");
        brush1Btn->setEnabled(false);
        brush1Btn->setGeometry(QRect(20, 200, 31, 24));
        QFont font;
        font.setFamilies({QString::fromUtf8("Rockwell")});
        brush1Btn->setFont(font);
        brush1Btn->setStyleSheet(QString::fromUtf8("background-color: rgb(170, 170, 255);"));
        brush4Btn = new QPushButton(centralwidget);
        brush4Btn->setObjectName("brush4Btn");
        brush4Btn->setGeometry(QRect(140, 200, 31, 24));
        brush4Btn->setFont(font);
        brush4Btn->setStyleSheet(QString::fromUtf8("background-color: rgb(170, 170, 255);"));
        brush2Btn = new QPushButton(centralwidget);
        brush2Btn->setObjectName("brush2Btn");
        brush2Btn->setGeometry(QRect(60, 200, 31, 24));
        brush2Btn->setFont(font);
        brush2Btn->setStyleSheet(QString::fromUtf8("background-color: rgb(170, 170, 255);"));
        brush3Btn = new QPushButton(centralwidget);
        brush3Btn->setObjectName("brush3Btn");
        brush3Btn->setGeometry(QRect(100, 200, 31, 24));
        brush3Btn->setFont(font);
        brush3Btn->setStyleSheet(QString::fromUtf8("background-color: rgb(170, 170, 255);"));
        tileButton = new QPushButton(centralwidget);
        tileButton->setObjectName("tileButton");
        tileButton->setGeometry(QRect(30, 230, 41, 41));
        tileButton->setStyleSheet(QString::fromUtf8("background-color: rgb(170, 170, 255);"));
        fillButton = new QPushButton(centralwidget);
        fillButton->setObjectName("fillButton");
        fillButton->setGeometry(QRect(80, 230, 41, 41));
        fillButton->setStyleSheet(QString::fromUtf8("background-color: rgb(170, 170, 255);"));
        fpsCombo = new QComboBox(centralwidget);
        fpsCombo->setObjectName("fpsCombo");
        fpsCombo->setGeometry(QRect(920, 40, 81, 20));
        fpsCombo->setStyleSheet(QString::fromUtf8("background-color: rgb(170, 170, 255);"));
        fpsLabel = new QLabel(centralwidget);
        fpsLabel->setObjectName("fpsLabel");
        fpsLabel->setGeometry(QRect(860, 40, 81, 20));
        fpsLabel->setFont(font);
        fpsLabel->setLayoutDirection(Qt::LeftToRight);
        fpsLabel->setAlignment(Qt::AlignCenter);
        saveButton = new QPushButton(centralwidget);
        saveButton->setObjectName("saveButton");
        saveButton->setGeometry(QRect(210, 540, 80, 31));
        saveButton->setStyleSheet(QString::fromUtf8("background-color: rgb(170, 170, 255);s"));
        loadButton = new QPushButton(centralwidget);
        loadButton->setObjectName("loadButton");
        loadButton->setGeometry(QRect(300, 540, 81, 31));
        loadButton->setStyleSheet(QString::fromUtf8("background-color: rgb(170, 170, 255);"));
        setSpriteSizeButton = new QPushButton(centralwidget);
        setSpriteSizeButton->setObjectName("setSpriteSizeButton");
        setSpriteSizeButton->setGeometry(QRect(610, 540, 111, 31));
        setSpriteSizeButton->setStyleSheet(QString::fromUtf8("background-color: rgb(170, 170, 255);"));
        canvasWidget = new Canvas(centralwidget);
        canvasWidget->setObjectName("canvasWidget");
        canvasWidget->setGeometry(QRect(210, 20, 512, 512));
        canvasWidget->setStyleSheet(QString::fromUtf8("gridline-color: rgb(213, 213, 213);"));
        zoomInButton = new QPushButton(centralwidget);
        zoomInButton->setObjectName("zoomInButton");
        zoomInButton->setGeometry(QRect(80, 280, 41, 41));
        zoomInButton->setStyleSheet(QString::fromUtf8("background-color: rgb(255, 170, 255);"));
        zoomOutButton = new QPushButton(centralwidget);
        zoomOutButton->setObjectName("zoomOutButton");
        zoomOutButton->setGeometry(QRect(130, 280, 41, 41));
        zoomOutButton->setStyleSheet(QString::fromUtf8("background-color: rgb(255, 170, 255);"));
        currentFrame = new QLabel(centralwidget);
        currentFrame->setObjectName("currentFrame");
        currentFrame->setGeometry(QRect(10, 330, 141, 31));
        QFont font1;
        font1.setFamilies({QString::fromUtf8("Rockwell")});
        font1.setPointSize(10);
        currentFrame->setFont(font1);
        lastFrame = new QPushButton(centralwidget);
        lastFrame->setObjectName("lastFrame");
        lastFrame->setEnabled(false);
        lastFrame->setGeometry(QRect(10, 360, 81, 29));
        lastFrame->setStyleSheet(QString::fromUtf8("background-color: rgb(85, 170, 255);"));
        nextFrame = new QPushButton(centralwidget);
        nextFrame->setObjectName("nextFrame");
        nextFrame->setEnabled(false);
        nextFrame->setGeometry(QRect(120, 360, 83, 29));
        nextFrame->setStyleSheet(QString::fromUtf8("background-color: rgb(85, 170, 255);"));
        deleteCurrentFrame = new QPushButton(centralwidget);
        deleteCurrentFrame->setObjectName("deleteCurrentFrame");
        deleteCurrentFrame->setEnabled(false);
        deleteCurrentFrame->setGeometry(QRect(10, 410, 191, 29));
        deleteCurrentFrame->setStyleSheet(QString::fromUtf8("background-color: rgb(85, 170, 255);"));
        addFrame = new QPushButton(centralwidget);
        addFrame->setObjectName("addFrame");
        addFrame->setGeometry(QRect(10, 460, 191, 29));
        addFrame->setStyleSheet(QString::fromUtf8("background-color: rgb(85, 170, 255);"));
        previewWidget = new Preview(centralwidget);
        previewWidget->setObjectName("previewWidget");
        previewWidget->setGeometry(QRect(730, 70, 256, 256));
        previewWidget->setStyleSheet(QString::fromUtf8("background-color: rgb(255, 255, 255);\n"
""));
        playButton = new QPushButton(centralwidget);
        playButton->setObjectName("playButton");
        playButton->setGeometry(QRect(730, 43, 21, 21));
        QFont font2;
        font2.setPointSize(12);
        playButton->setFont(font2);
        playButton->setStyleSheet(QString::fromUtf8("background-color: rgb(170, 170, 255);"));
        pauseButton = new QPushButton(centralwidget);
        pauseButton->setObjectName("pauseButton");
        pauseButton->setGeometry(QRect(750, 43, 21, 21));
        pauseButton->setStyleSheet(QString::fromUtf8("background-color: rgb(170, 170, 255);"));
        previewLabel = new QLabel(centralwidget);
        previewLabel->setObjectName("previewLabel");
        previewLabel->setGeometry(QRect(730, 10, 101, 31));
        QFont font3;
        font3.setFamilies({QString::fromUtf8("Rockwell")});
        font3.setPointSize(15);
        previewLabel->setFont(font3);
        previewLabel->setStyleSheet(QString::fromUtf8(""));
        brushEraserSize = new QLabel(centralwidget);
        brushEraserSize->setObjectName("brushEraserSize");
        brushEraserSize->setGeometry(QRect(10, 180, 141, 21));
        brushEraserSize->setFont(font1);
        smallPreviewButton = new QPushButton(centralwidget);
        smallPreviewButton->setObjectName("smallPreviewButton");
        smallPreviewButton->setGeometry(QRect(771, 43, 84, 21));
        smallPreviewButton->setStyleSheet(QString::fromUtf8("background-color: rgb(170, 170, 255);"));
        brushButton = new QPushButton(centralwidget);
        brushButton->setObjectName("brushButton");
        brushButton->setGeometry(QRect(130, 230, 41, 41));
        brushButton->setStyleSheet(QString::fromUtf8("background-color: rgb(170, 170, 255);"));
        colorPickBtn = new QPushButton(centralwidget);
        colorPickBtn->setObjectName("colorPickBtn");
        colorPickBtn->setGeometry(QRect(10, 140, 181, 31));
        colorSelector = new QLabel(centralwidget);
        colorSelector->setObjectName("colorSelector");
        colorSelector->setGeometry(QRect(10, 110, 111, 31));
        colorSelector->setFont(font1);
        eraserButton = new QPushButton(centralwidget);
        eraserButton->setObjectName("eraserButton");
        eraserButton->setGeometry(QRect(30, 280, 41, 41));
        eraserButton->setStyleSheet(QString::fromUtf8("background-color: rgb(255, 170, 255);"));
        duplicateFrame = new QPushButton(centralwidget);
        duplicateFrame->setObjectName("duplicateFrame");
        duplicateFrame->setGeometry(QRect(10, 510, 191, 29));
        duplicateFrame->setStyleSheet(QString::fromUtf8("background-color: rgb(85, 170, 255);"));
        clearFrame = new QPushButton(centralwidget);
        clearFrame->setObjectName("clearFrame");
        clearFrame->setGeometry(QRect(10, 560, 191, 29));
        clearFrame->setStyleSheet(QString::fromUtf8("background-color: rgb(85, 170, 255);"));
        MainWindow->setCentralWidget(centralwidget);
        menubar = new QMenuBar(MainWindow);
        menubar->setObjectName("menubar");
        menubar->setGeometry(QRect(0, 0, 1298, 21));
        MainWindow->setMenuBar(menubar);
        statusbar = new QStatusBar(MainWindow);
        statusbar->setObjectName("statusbar");
        MainWindow->setStatusBar(statusbar);

        retranslateUi(MainWindow);

        QMetaObject::connectSlotsByName(MainWindow);
    } // setupUi

    void retranslateUi(QMainWindow *MainWindow)
    {
        MainWindow->setWindowTitle(QCoreApplication::translate("MainWindow", "MainWindow", nullptr));
#if QT_CONFIG(tooltip)
        MainWindow->setToolTip(QString());
#endif // QT_CONFIG(tooltip)
        brush1Btn->setText(QCoreApplication::translate("MainWindow", "1", nullptr));
        brush4Btn->setText(QCoreApplication::translate("MainWindow", "4", nullptr));
        brush2Btn->setText(QCoreApplication::translate("MainWindow", "2", nullptr));
        brush3Btn->setText(QCoreApplication::translate("MainWindow", "3", nullptr));
        tileButton->setText(QString());
        fillButton->setText(QString());
        fpsLabel->setText(QCoreApplication::translate("MainWindow", "FPS", nullptr));
        saveButton->setText(QCoreApplication::translate("MainWindow", "Save Sprite", nullptr));
        loadButton->setText(QCoreApplication::translate("MainWindow", "Load Sprite", nullptr));
        setSpriteSizeButton->setText(QCoreApplication::translate("MainWindow", "Set Size", nullptr));
        zoomInButton->setText(QString());
        zoomOutButton->setText(QString());
        currentFrame->setText(QCoreApplication::translate("MainWindow", "Frame: 1", nullptr));
        lastFrame->setText(QCoreApplication::translate("MainWindow", "Last Frame", nullptr));
        nextFrame->setText(QCoreApplication::translate("MainWindow", "Next Frame", nullptr));
        deleteCurrentFrame->setText(QCoreApplication::translate("MainWindow", "Delete Current Frame", nullptr));
        addFrame->setText(QCoreApplication::translate("MainWindow", "Add Frame", nullptr));
        playButton->setText(QString());
        pauseButton->setText(QString());
        previewLabel->setText(QCoreApplication::translate("MainWindow", "Preview", nullptr));
        brushEraserSize->setText(QCoreApplication::translate("MainWindow", "Brush/Eraser Size", nullptr));
        smallPreviewButton->setText(QCoreApplication::translate("MainWindow", "Preview Actual", nullptr));
        brushButton->setText(QString());
        colorPickBtn->setText(QString());
        colorSelector->setText(QCoreApplication::translate("MainWindow", "Color Selector", nullptr));
        eraserButton->setText(QString());
        duplicateFrame->setText(QCoreApplication::translate("MainWindow", "Duplicate Frame", nullptr));
        clearFrame->setText(QCoreApplication::translate("MainWindow", "Clear Frame", nullptr));
    } // retranslateUi

};

namespace Ui {
    class MainWindow: public Ui_MainWindow {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_MAINWINDOW_H
