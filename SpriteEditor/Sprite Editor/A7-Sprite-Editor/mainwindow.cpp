#include "mainwindow.h"
#include "ui_mainwindow.h"

///
/// \brief Constructs the main window of the application.
/// Sets up the user interface, adds FPS options to the combo box,
/// sets up icons and stylesheets for buttons, and connects signals
/// and slots for various buttons and widgets.
///
/// \param  parent The parent widget
///
MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , m_ui(new Ui::MainWindow)
{
    m_ui->setupUi(this);

    // Adds numbers to the FPS combo box
    m_ui->fpsCombo->addItem("1");
    m_ui->fpsCombo->addItem("15");
    m_ui->fpsCombo->addItem("30");
    m_ui->fpsCombo->addItem("45");
    m_ui->fpsCombo->addItem("60");

    // Disable pause button
    m_ui->pauseButton->setEnabled(false);

    // Sets up icons and stylesheets
    m_ui->fillButton->setIcon(QIcon(":/buttonIcons/Resources/paint-bucket.png"));
    m_ui->brushButton->setIcon(QIcon(":/buttonIcons/Resources/paint-brush.png"));
    m_ui->zoomInButton->setIcon(QIcon(":/buttonIcons/Resources/zoom-icon-png-8436.png"));
    m_ui->zoomOutButton->setIcon(QIcon(":/buttonIcons/Resources/zoom-out.png"));
    m_ui->tileButton->setIcon(QIcon(":/buttonIcons/Resources/tiles.png"));
    m_ui->playButton->setIcon(QIcon(":/buttonIcons/Resources/play-button.png"));
    m_ui->pauseButton->setIcon(QIcon(":/buttonIcons/Resources/pause-button.png"));
    m_ui->eraserButton->setIcon(QIcon(":/buttonIcons/Resources/eraser.png"));
    m_ui->colorPickBtn->setStyleSheet(QString("QPushButton{background-color: rgb(0,0,0);}"));

    // Connects frame buttons
    connect(m_ui->addFrame, &QPushButton::clicked, m_ui->canvasWidget, &Canvas::on_addFrameClicked);
    connect(m_ui->lastFrame, &QPushButton::clicked, m_ui->canvasWidget, &Canvas::on_lastFrameClicked);
    connect(m_ui->nextFrame, &QPushButton::clicked, m_ui->canvasWidget, &Canvas::on_nextFrameClicked);
    connect(m_ui->deleteCurrentFrame, &QPushButton::clicked, m_ui->canvasWidget, &Canvas::on_deleteCurrentFrameClicked);
    connect(m_ui->duplicateFrame, &QPushButton::clicked, m_ui->canvasWidget, &Canvas::on_duplicateFrameClicked);
    connect(m_ui->clearFrame, &QPushButton::clicked, m_ui->canvasWidget, &Canvas::on_clearFrameClicked);

    // Connects frame button signals
    connect(m_ui->canvasWidget, &Canvas::enableLastButton, this, &MainWindow::enableLastButton);
    connect(m_ui->canvasWidget, &Canvas::disableLastButton, this, &MainWindow::disableLastButton);
    connect(m_ui->canvasWidget, &Canvas::enableNextButton, this, &MainWindow::enableNextButton);
    connect(m_ui->canvasWidget, &Canvas::disableNextButton, this, &MainWindow::disableNextButton);
    connect(m_ui->canvasWidget, &Canvas::enableDeleteButton, this, &MainWindow::enableDeleteButton);
    connect(m_ui->canvasWidget, &Canvas::disableDeleteButton, this, &MainWindow::disableDeleteButton);
    connect(m_ui->canvasWidget, &Canvas::updateFrameNumber, this, &MainWindow::updateFrameNumber);

    // Connect brush/size buttons
    connect(m_ui->brushButton, &QPushButton::clicked, m_ui->canvasWidget, &Canvas::setBrush);
    connect(m_ui->brush1Btn, &QPushButton::clicked, m_ui->canvasWidget, &Canvas::setBrushAndEraserSize1);
    connect(m_ui->brush2Btn, &QPushButton::clicked, m_ui->canvasWidget, &Canvas::setBrushAndEraserSize2);
    connect(m_ui->brush3Btn, &QPushButton::clicked, m_ui->canvasWidget, &Canvas::setBrushAndEraserSize3);
    connect(m_ui->brush4Btn, &QPushButton::clicked, m_ui->canvasWidget, &Canvas::setBrushAndEraserSize4);
    connect(m_ui->fillButton, &QPushButton::clicked, this, &MainWindow::disableAllSizeButtons);
    connect(m_ui->tileButton, &QPushButton::clicked, this, &MainWindow::disableAllSizeButtons);
    connect(m_ui->brushButton, &QPushButton::clicked, this, &MainWindow::enableAllSizeButtons);

    connect(m_ui->brush1Btn, &QPushButton::clicked, this, &MainWindow::enableAllSizeButtons);
    connect(m_ui->brush1Btn, &QPushButton::clicked, this, &MainWindow::disableSizeButton1);
    connect(m_ui->brush2Btn, &QPushButton::clicked, this, &MainWindow::enableAllSizeButtons);
    connect(m_ui->brush2Btn, &QPushButton::clicked, this, &MainWindow::disableSizeButton2);
    connect(m_ui->brush3Btn, &QPushButton::clicked, this, &MainWindow::enableAllSizeButtons);
    connect(m_ui->brush3Btn, &QPushButton::clicked, this, &MainWindow::disableSizeButton3);
    connect(m_ui->brush4Btn, &QPushButton::clicked, this, &MainWindow::enableAllSizeButtons);
    connect(m_ui->brush4Btn, &QPushButton::clicked, this, &MainWindow::disableSizeButton4);

    connect(m_ui->brushButton, &QPushButton::clicked, this, &MainWindow::disableSizeButton1);

    //  Connect other tool Buttons
    connect(m_ui->eraserButton, &QPushButton::clicked, m_ui->canvasWidget, &Canvas::setEraser);
    connect(m_ui->eraserButton, &QPushButton::clicked, this, &MainWindow::enableAllSizeButtons);
    connect(m_ui->eraserButton, &QPushButton::clicked, this, &MainWindow::disableSizeButton1);
    connect(m_ui->tileButton, &QPushButton::clicked, m_ui->canvasWidget, &Canvas::setTile);
    connect(m_ui->fillButton, &QPushButton::clicked, m_ui->canvasWidget, &Canvas::setBucket);
    connect(m_ui->zoomInButton, &QPushButton::clicked, m_ui->canvasWidget, &Canvas::zoomIn);
    connect(m_ui->zoomOutButton, &QPushButton::clicked, m_ui->canvasWidget, &Canvas::zoomOut);

    // Connect fps combo box values
    connect(m_ui->fpsCombo, &QComboBox::currentIndexChanged, m_ui->previewWidget, &Preview::changeFPS);

    // Connections for updating preview
    connect(m_ui->playButton, &QPushButton::clicked, m_ui->previewWidget, &Preview::setPlaybackTrue);
    connect(m_ui->pauseButton, &QPushButton::clicked, m_ui->previewWidget, &Preview::setPlaybackFalse);
    connect(m_ui->canvasWidget, &Canvas::updatePreview, m_ui->previewWidget, &Preview::updatePreview);
    connect(m_ui->playButton, &QPushButton::clicked, m_ui->canvasWidget, &::Canvas::on_playButtonClicked);

    // Connections for pausing and playing preview animation
    connect(m_ui->playButton, &QPushButton::clicked, this, &MainWindow::enablePauseDisablePlay);
    connect(m_ui->pauseButton, &QPushButton::clicked, this, &MainWindow::disablePause);
    connect(m_ui->pauseButton, &QPushButton::clicked, this, &MainWindow::enablePlayWithTimer);
    connect(m_ui->deleteCurrentFrame, &QPushButton::clicked, m_ui->previewWidget, &Preview::setPlaybackFalse);
    connect(m_ui->deleteCurrentFrame, &QPushButton::clicked, this, &MainWindow::disablePause);
    connect(m_ui->deleteCurrentFrame, &QPushButton::clicked, this, &MainWindow::enablePlay);
    connect(m_ui->addFrame, &QPushButton::clicked, m_ui->previewWidget, &Preview::setPlaybackFalse);
    connect(m_ui->addFrame, &QPushButton::clicked, this, &MainWindow::disablePause);
    connect(m_ui->addFrame, &QPushButton::clicked, this, &MainWindow::enablePlay);

    connect(m_ui->smallPreviewButton, &QPushButton::clicked, m_ui->previewWidget, &Preview::actualSize);
    connect(m_ui->setSpriteSizeButton, &QPushButton::clicked, m_ui->canvasWidget, &Canvas::on_setSpriteSizeClicked);

    //  Connects changing cursor icon
    connect(m_ui->fillButton, &QPushButton::clicked, this, [this]{this->setCursor(QCursor(QPixmap(":/buttonIcons/Resources/paint-bucket.png").scaled(QSize(25,25), Qt::IgnoreAspectRatio)));} );
    connect(m_ui->tileButton, &QPushButton::clicked, this, [this]{this->setCursor(QCursor(QPixmap(":/buttonIcons/Resources/tiles.png").scaled(QSize(25,25), Qt::IgnoreAspectRatio)));} );
    connect(m_ui->eraserButton, &QPushButton::clicked, this, [this]{this->setCursor(QCursor(QPixmap(":/buttonIcons/Resources/eraser.png").scaled(QSize(25,25), Qt::IgnoreAspectRatio)));} );
    connect(m_ui->brushButton, &QPushButton::clicked, this, [this]{this->setCursor(QCursor(Qt::ArrowCursor));});

    //  Connects saving and loading
    connect(m_ui->saveButton, &QPushButton::clicked, m_ui->canvasWidget, &Canvas::on_SaveClicked);
    connect(m_ui->loadButton, &QPushButton::clicked, m_ui->canvasWidget, &Canvas::on_LoadClicked);

    //  Connects changing of the color displayed
    connect(m_ui->colorPickBtn, &QPushButton::clicked, m_ui->canvasWidget, &Canvas::setColor);
    connect(m_ui->canvasWidget, &Canvas::changeColorButton, this, &MainWindow::changeColorButton);
}


/// \brief Destructor for the MainWindow class. Cleans up the allocated memory for the UI.
MainWindow::~MainWindow() {
    delete m_ui;
}

/// \brief Enables the Last Frame button.
void MainWindow::enableLastButton() {
    m_ui->lastFrame->setEnabled(true);
}

/// \brief Disables the "Last Frame" button.
void MainWindow::disableLastButton(){
    m_ui->lastFrame->setEnabled(false);
}

/// \brief Enables the "Next Frame" button.
void MainWindow::enableNextButton(){
    m_ui->nextFrame->setEnabled(true);
}

/// \brief Disables the "Next Frame" button.
void MainWindow::disableNextButton(){
    m_ui->nextFrame->setEnabled(false);
}

/// \brief Enables the "Delete Frame" button.
void MainWindow::enableDeleteButton(){
    m_ui->deleteCurrentFrame->setEnabled(true);
}

/// \brief Disables the "Delete Frame" button.
void MainWindow::disableDeleteButton(){
    m_ui->deleteCurrentFrame->setEnabled(false);
}

/// \brief Enables size buttons.
void MainWindow::enableAllSizeButtons(){
    m_ui->brush1Btn->setEnabled(true);
    m_ui->brush2Btn->setEnabled(true);
    m_ui->brush3Btn->setEnabled(true);
    m_ui->brush4Btn->setEnabled(true);
}

/// \brief Disables all size buttons.
void MainWindow::disableAllSizeButtons(){
    m_ui->brush1Btn->setEnabled(false);
    m_ui->brush2Btn->setEnabled(false);
    m_ui->brush3Btn->setEnabled(false);
    m_ui->brush4Btn->setEnabled(false);
}

/// \brief Disables the first size button.
void MainWindow::disableSizeButton1(){
    m_ui->brush1Btn->setEnabled(false);
}

/// \brief Disables the second size button.
void MainWindow::disableSizeButton2(){
    m_ui->brush2Btn->setEnabled(false);
}

/// \brief Disables the third size button.
void MainWindow::disableSizeButton3(){
    m_ui->brush3Btn->setEnabled(false);
}

/// \brief Disables the fourth size button.
void MainWindow::disableSizeButton4(){
    m_ui->brush4Btn->setEnabled(false);
}

/// \brief Disables the "Pause" button.
void MainWindow::disablePause(){
    m_ui->pauseButton->setEnabled(false);
}

void MainWindow::enablePauseDisablePlay(){
    m_ui->pauseButton->setEnabled(true);
    m_ui->playButton->setEnabled(false);
}

/// \brief Enables the "Play" button after a set time.
void MainWindow::enablePlayWithTimer(){
    QTimer::singleShot(600, this, &MainWindow::enablePlay);
}

/// \brief Enables the "Play" button.
void MainWindow::enablePlay(){
    m_ui->playButton->setEnabled(true);
}

///
/// \brief Updates the displayed frame number.
/// \param frameNum The new frame number to display.
///
void MainWindow::updateFrameNumber(int frameNum){
    m_ui->currentFrame->clear();
    m_ui->currentFrame->setText(QString::fromStdString("Frame: " + std::to_string(frameNum + 1)));
}

///
/// \brief Changes the color of the "Color Picker" button.
/// \param = color The new color for the button.
///
void MainWindow::changeColorButton(QString color){
    m_ui->colorPickBtn->setStyleSheet(color);
}
