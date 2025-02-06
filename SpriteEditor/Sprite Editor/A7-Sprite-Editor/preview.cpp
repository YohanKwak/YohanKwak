#include "preview.h"

///
/// \brief The constructor for preview. Takes in a QWidget as its parent
/// \param parent The parent widget
///
Preview::Preview(QWidget *parent): QWidget(parent), m_playback(false), m_displayActual(false),
    m_scale(1), m_frameRate(1), m_previewIndex(0){
}

///
/// \brief Paints the preview. The size is either
/// the larger normal preview or the smaller actual sprite size
/// \param event
///
void Preview::paintEvent(QPaintEvent *event){
    QPainter painter(this);
    QRect oldRect = event->rect();
    if(m_displayActual){
            QRect newRect = event->rect();
            QPoint centeredPoint = QPoint(((256 - m_previewImage.width()) / 2),(256 - m_previewImage.height()) / 2);
            newRect.setTopLeft(centeredPoint);
            QSize scaledSize = QSize(m_previewImage.width(), m_previewImage.height());
            painter.drawImage(newRect, m_previewImage.scaled(scaledSize, Qt::KeepAspectRatio, Qt::FastTransformation), oldRect);
    }
    else{
        painter.drawImage(oldRect, m_scaledPreview, oldRect);
    }
}

///
/// \brief Sets preview to reflect changes to the frames
/// \param newFrames, the updated frames
/// \param index, which frame is currently displayed on canvas
///
void Preview::updatePreview(QVector<QImage> newFrames, int index){
    m_previewIndex = index;
    if(m_playback == true){
        m_frames = newFrames;
        m_previewImage = m_frames.at(m_previewIndex).copy();
        m_scaledPreview = m_previewImage.copy();
        m_scale = qMax(m_scaledPreview.width(), m_scaledPreview.height());
        m_scale = 256 / m_scale;
        m_scaledPreview = m_scaledPreview.scaled(256, 256, Qt::IgnoreAspectRatio, Qt::FastTransformation);
        update();
        swapPreview();
    }
}

///
/// \brief Displays and iterates through the frames
///
void Preview::swapPreview(){
    if(m_playback == true){
    if(m_previewIndex >= m_frames.size()){
        m_previewIndex = 0;
    }
    m_previewImage = m_frames.at(m_previewIndex).copy();
    m_scaledPreview = m_previewImage.copy();
    m_scaledPreview = m_scaledPreview.scaled(256, 256, Qt::KeepAspectRatio, Qt::FastTransformation);
    m_previewIndex++;
    update();
    QTimer::singleShot((1000)/m_frameRate, this, &Preview::swapPreview);
    }
}

///
/// \brief Starts the animation when play button is pressed
///
void Preview::setPlaybackTrue(){
    m_playback = true;
}

///
/// \brief Stops the animation when pause button is pressed, or the
/// frames change
///
void Preview::setPlaybackFalse(){
    m_playback = false;
}

///
/// \brief Changes the rate at which the preview plays
/// \param index position of desired FPS in dropdown menu
///
void Preview::changeFPS(int index){
    if(index == 0){
        m_frameRate = 1;
        return;
    }
    m_frameRate = index * 15;
}

///
/// \brief Changes display to/from actual size
///
void Preview::actualSize(){
    if(m_displayActual){
        m_displayActual = false;
    }
    else{
        m_displayActual = true;
    }
    update();
}
