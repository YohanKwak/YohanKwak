#ifndef PREVIEW_H
#define PREVIEW_H

#include <QObject>
#include <QWidget>
#include <QPaintEvent>
#include <QImage>
#include <QPainter>
#include <QVector>
#include <QTimer>

///
/// \brief The preview class
///
/// \authors Miguel Mendoza, Matt Rogers, Logan Hunter,
/// Amelia Smith, Yohan Kwak, Yamin Zhuang
///
/// Reviewed by Matt Rogers, Miguel Mendoza
///
class Preview : public  QWidget{
    Q_OBJECT
public:
    explicit Preview(QWidget *parent = nullptr);
protected:
    ///
    /// \brief Paints the preview. The size is either
    /// the larger normal preview or the smaller actual sprite size
    /// \param event
    ///
    void paintEvent(QPaintEvent *event) override;

private:
    QVector<QImage> m_frames; // Vector of QImages to hold sprites
    QImage m_previewImage; // Current image being displayed in m_frames
    QImage m_scaledPreview; // Scaled current image displayed in preview window.
    QSize m_spriteSize; // Sprite size of current image.
    bool m_playback; // Bool to determine if preview animation should be playing.
    bool m_displayActual; // Bool to determine if the preview should be shown at its actual size.
    int m_scale; // Scale of preview window width divided by sprite size.
    int m_frameRate; // Frame rate of preview animation.
    int m_previewIndex; // Current index of image to preview.

public slots:
    /// \brief Paints the preview. The size is either
    /// the larger normal preview or the smaller actual sprite size
    /// \param event
    void updatePreview(QVector<QImage>, int index);

    /// \brief Displays and iterates through the frames
    void swapPreview();

    /// \brief For when play button is pressed
    void setPlaybackTrue();

    /// \brief For when pause button is pressed, or the frames change
    void setPlaybackFalse();

    /// \brief Changes the rate at which the preview plays
    /// \param index, position of desired FPS in dropdown menu
    void changeFPS(int index);

    /// \brief Changes display to/from actual size
    void actualSize();
};

#endif // PREVIEW_H
