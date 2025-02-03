#ifndef CANVAS_H
#define CANVAS_H

#include <QObject>
#include <QPainter>
#include <QColorDialog>
#include <QInputDialog>
#include <QPaintEvent>
#include <QWidget>
#include <QDebug>
#include <QFile>
#include <QFileDialog>
#include <QJsonDocument>
#include <QJsonObject>
#include <QJsonArray>
#include <QMessageBox>
#include <string>
#include <queue>

///
/// \brief The canvas class is a promoted QWidget that stores all data and methods necessary for
/// storing and drawing images. It is the model in the MVC that contains all the logic for
/// mouse events, frame storage, pixel modifcation, tool selection and utilities, updating the preview,
/// and more.
///
/// \authors Miguel Mendoza, Matt Rogers, Logan Hunter,
/// Amelia Smith, Yohan Kwak, Yamin Zhuang
///
/// Reviewed by Amelia Smith, Logan Hunter
///
class Canvas : public  QWidget
{
    Q_OBJECT
public:
    ///
    /// \brief Constructor for Canvas. Takes in a QWidget as its parent.
    /// \param parent
    ///
    explicit Canvas(QWidget *parent = nullptr);

protected:
    ///
    /// \brief Prompts the draw method on the point that was pressed.
    /// \param event = Mouse button pressed
    ///
    void mousePressEvent(QMouseEvent *event) override;

    ///
    /// \brief Prompts the draw method when the mouse moves if
    ///  the left mouse button is held down.
    /// \param event = Mouse moving
    ///
    void mouseMoveEvent(QMouseEvent *event) override;

    ///
    /// \brief Prompts the draw method at the point where
    /// the user released the mouse button and stops drawing.
    /// \param event = Mouse released
    ///
    void mouseReleaseEvent(QMouseEvent *event) override;

    ///
    /// \brief Paints a 512x512 rectangle scaled by the images width and height.
    /// If zoomed in, the rectangle is scaled by the zoom factor instead of 512x512.
    /// \param event
    ///
    void paintEvent(QPaintEvent *event) override;

    ///
    /// \brief Draws onto the canvas by modifying the image based on mousePoint.
    /// \param mousePoint = Given mouse cursor point.
    ///
    void draw(const QPoint &endPoint);

    ///
    /// \brief Helper method for the bucket tool that uses the flood fill algorithm.
    /// \param x = Mouse point's x position
    /// \param y = Mouse point's y position
    ///
    void bucketTool(int x, int y);

    ///
    /// \brief Creates a default gray checkered background when a new frame
    /// is added.
    ///
    void setDefaultBackground();

    ///
    /// \brief Helper method to save the current sprite image vector into a JSON formatted
    /// .ssp file. Loops through every row and column of each image and adds the rgba value
    /// of every pixel into a 2D array that makes an image. Adds the frames, height, width,
    /// and number of frames to each file.
    ///
    void saveProject(QFile &file);

    ///
    /// \brief Helper method to load a sprite image vector from a JSON formatted array.
    ///
    /// \param file = .ssp file to load onto drawing canvas
    ///
    void loadProject(QFile &file);

    ///
    /// \brief Helper method to scale the current image to size.
    ///
    void copyAndScaleImage();

    ///
    /// \brief Helper method to scale the default background image to size.
    ///
    void copyAndScaleDefaultImage();

private:
    QVector<QImage> m_frames; ///Vector that stores all the frames
    QImage m_defaultImage; ///Stores the default background
    QImage m_spriteImage; ///Stores a version of the current frame image
    QImage m_scaledImage; ///Stores a scaled version of the current frame image
    QImage m_scaledDefaultBackground; ///Stores a scaled version of the default bakground
    QSize m_spriteSize; ///Stores the size of the sprite
    QPoint m_lastMousePoint; ///Stores the value where the mouse was last recorded at
    QColor m_currentColor; ///Stores the current color of the brush
    QColor m_colorToReplace; ///Stores the color that will be replacing the current color
    QString m_currentTool; ///Stores the current tool as a string
    bool m_unsaved = false; ///Stores if the drawing is unsaved or saved
    bool m_isDrawing = false; ///Stores if the user is currently is drawing
    bool m_isModified() const {return m_unsaved;} ///Stores if the drawing has been modified since the last save
    int m_brushAndEraserSize; ///Stores the current brush or eraser size
    int m_imageScale; ///Stores the current scale of the image
    int m_zoomScale; ///Stores the current zoom scale of the image
    int m_frameRate; ///Stores the current framerate for the animation
    int m_currentFrameIndex; ///Stores the current index of the current frame of the animation

public slots:
    ///
    /// \brief Adds a frame to the sprite image vector. Initializes with a default
    /// background.
    ///
    void on_addFrameClicked();

    ///
    /// \brief Deletes this frame from the sprite image vector.
    /// Replaces current image with next available image based on vector
    /// deletion convention.
    ///
    void on_deleteCurrentFrameClicked();

    ///
    /// \brief Sets the current frame to the previous image in the sprite image vector.
    ///
    void on_lastFrameClicked();

    ///
    /// \brief Sets the current frame to the next image in the sprite image vector.
    ///
    void on_nextFrameClicked();

    ///
    /// \brief Creates a duplicate frame and adds it to the sprite image vector.
    ///
    void on_duplicateFrameClicked();

    ///
    /// \brief Clears the current sprite image.
    ///
    void on_clearFrameClicked();

    ///
    /// \brief Clears any current sprite images and sets the new
    /// sprite image size as chosen.
    ///
    void on_setSpriteSizeClicked();

    ///
    /// \brief Cues the file to be saved in an .ssp format to whatever
    /// file path the user chooses.
    ///
    void on_SaveClicked();

    ///
    /// \brief Cues the file to be loaded from an .ssp file into a modifiable
    /// sprite vector that appears on the drawing canvas.
    ///
    void on_LoadClicked();

    ///
    /// \brief Starts the preview animation.
    ///
    void on_playButtonClicked();

    ///
    /// \brief Sets the brush and eraser size to 1.
    ///
    void setBrushAndEraserSize1();

    ///
    /// \brief Sets the brush and eraser size to 2.
    ///
    void setBrushAndEraserSize2();

    ///
    /// \brief Sets the brush and eraser size to 3.
    ///
    void setBrushAndEraserSize3();

    ///
    /// \brief Sets the brush and eraser size to 4.
    ///
    void setBrushAndEraserSize4();

    ///
    /// \brief Sets the current tool to brush.
    ///
    void setBrush();

    ///
    /// \brief Sets the current tool to bucket.
    ///
    void setBucket();

    ///
    /// \brief Sets the current tool to eraser.
    ///
    void setEraser();

    ///
    /// \brief Sets the current tool to tile.
    ///
    void setTile();

    ///
    /// \brief Sets the current color as chosen by the user.
    ///
    void setColor();

    ///
    /// \brief Zooms the drawing canvas in.
    ///
    void zoomIn();

    ///
    /// \brief Zooms the drawing canvas out.
    ///
    void zoomOut();

signals:
    void updatePreview(QVector<QImage> frames, int index); ///Sends a signal to update the preview
    void changeColorButton(QString color); ///Sends a signal to update the color button
    void updateFrameNumber(int frameNum); ///Sends a signal to update the frame number
    void enableLastButton(); ///Sends a signal to enable the last frame button
    void disableLastButton(); ///Sends a signal to disable the last frame button
    void enableNextButton(); ///Sends a signal to enable the next frame button
    void disableNextButton(); ///Sends a signal to disable the next frame button
    void enableDeleteButton(); ///Sends a signal to enable the delete frame button
    void disableDeleteButton(); ///Sends a signal to disenable the delete frame button
};

#endif // CANVAS_H
