#include "canvas.h"

///
/// \brief Constructor for Canvas. Takes in a QWidget as its parent.
/// \param parent
///
Canvas::Canvas(QWidget *parent)
    : QWidget(parent), m_spriteSize(QSize(16, 16)), m_currentColor(Qt::black), m_unsaved(false)
    , m_isDrawing(false), m_brushAndEraserSize(1), m_currentFrameIndex(0)
{
    m_currentTool = "Pen";
    setDefaultBackground();
    m_spriteImage = QImage(m_spriteSize, QImage::Format_ARGB32);
    m_spriteImage.fill(QColorConstants::Transparent);
    m_frames.append(m_spriteImage);
    m_scaledDefaultBackground = m_defaultImage.copy();
    m_scaledImage = m_spriteImage.copy();
    m_imageScale = 512 / m_spriteSize.width();
    m_zoomScale = m_imageScale;
    m_scaledDefaultBackground = m_scaledDefaultBackground.scaled(512, 512, Qt::IgnoreAspectRatio, Qt::FastTransformation);
    m_scaledImage = m_scaledImage.scaled(512, 512, Qt::IgnoreAspectRatio, Qt::FastTransformation);
    update();
}

///
/// \brief Prompts the draw method on the point that was pressed.
/// \param event = Mouse button pressed
///
void Canvas::mousePressEvent(QMouseEvent *event)
{
    if (event->button() == Qt::LeftButton) {
        m_lastMousePoint = event->position().toPoint();
        draw(m_lastMousePoint);
        m_isDrawing = true;
    }
}

///
/// \brief Prompts the draw method when the mouse moves if
///  the left mouse button is held down.
/// \param event = Mouse moving
///
void Canvas::mouseMoveEvent(QMouseEvent *event)
{
    if ((event->buttons() & Qt::LeftButton) && m_isDrawing)
        draw(event->position().toPoint());
}

///
/// \brief Prompts the draw method at the point where
/// the user released the mouse button and stops drawing.
/// \param event = Mouse released
///
void Canvas::mouseReleaseEvent(QMouseEvent *event)
{
    if (event->button() == Qt::LeftButton && m_isDrawing) {
        draw(event->position().toPoint());
        m_isDrawing = false;
    }
}

///
/// \brief Paints a 512x512 rectangle scaled by the images width and height.
/// If zoomed in, the rectangle is scaled by the zoom factor instead of 512x512.
/// \param event
///
void Canvas::paintEvent(QPaintEvent *event)
{
    QPainter painter(this);
    QRect oldRect = event->rect();
    QRect newRect = event->rect();
    QPoint centeredTopLeftPoint = QPoint(((512 - m_spriteImage.width() * m_zoomScale) / 2), (512 - m_spriteImage.height() * m_zoomScale) / 2);
    newRect.setTopLeft(centeredTopLeftPoint);
    newRect.setWidth((m_spriteImage.width() * m_zoomScale) * m_imageScale / m_zoomScale);
    newRect.setHeight((m_spriteImage.width() * m_zoomScale) * m_imageScale / m_zoomScale);
    QSize zoomScaledSize = QSize(m_spriteImage.width() * m_zoomScale, m_spriteImage.height() * m_zoomScale);
    painter.drawImage(newRect, m_defaultImage.scaled(zoomScaledSize, Qt::KeepAspectRatio, Qt::FastTransformation), oldRect);
    painter.drawImage(newRect, m_spriteImage.scaled(zoomScaledSize, Qt::KeepAspectRatio, Qt::FastTransformation), oldRect);
    update();
}

///
/// \brief Draws onto the canvas by modifying the image based on mousePoint.
/// \param mousePoint = Given mouse cursor point.
///
void Canvas::draw(const QPoint &mousePoint)
{
    QPoint originPoint = QPoint(((512 - m_spriteImage.width() * m_zoomScale) / 2), ((512 - m_spriteImage.height() * m_zoomScale) / 2));
    QPoint smallMousePoint;
    smallMousePoint.setX((mousePoint.x() - originPoint.x()) / (m_zoomScale));
    smallMousePoint.setY((mousePoint.y() - originPoint.y()) / (m_zoomScale));
    if(smallMousePoint.x() < 0){
        smallMousePoint.setX(0);
    }
    if(smallMousePoint.y() < 0){
        smallMousePoint.setY(0);
    }
    if (m_currentTool == "Bucket"){
        m_colorToReplace = m_spriteImage.pixelColor(smallMousePoint);
        bucketTool(smallMousePoint.x(), smallMousePoint.y());
    } else if (m_currentTool == "Tile"){
        // Keep brush within the drawing area.
        for(int x = smallMousePoint.x(); x < m_brushAndEraserSize + smallMousePoint.x() && x < m_spriteSize.width(); x++){
            for(int y = smallMousePoint.y(); y < m_brushAndEraserSize + smallMousePoint.y() && y < m_spriteSize.height(); y++){
                // Drawing in bottom left quadrant
                if(x - m_spriteSize.width() * 0.5 >= 0 && y + m_spriteSize.height() * 0.5 < m_spriteSize.height()){
                    m_spriteImage.setPixelColor(x,y,m_currentColor);
                    m_spriteImage.setPixelColor(x, y + m_spriteSize.height() * 0.5, m_currentColor);
                    m_spriteImage.setPixelColor(x - m_spriteSize.width() * 0.5, y, m_currentColor);
                    m_spriteImage.setPixelColor(x - m_spriteSize.width() * 0.5, y + m_spriteSize.height() * 0.5, m_currentColor);
                    m_frames.replace(m_currentFrameIndex, m_spriteImage);
                // Drawing in top left quadrant
                } else if (x - m_spriteSize.width() * 0.5 >= 0 ){
                    m_spriteImage.setPixelColor(x - m_spriteSize.width() * 0.5, y, m_currentColor);
                    m_spriteImage.setPixelColor(x,y,m_currentColor);
                // Drawing in top right quadrant
                } else if (y + m_spriteSize.height() * 0.5 < m_spriteSize.height()) {
                    m_spriteImage.setPixelColor(x,y,m_currentColor);
                    m_spriteImage.setPixelColor(x, y + m_spriteSize.height() * 0.5, m_currentColor);
                // Drawing in bottom right quadrant
                } else {
                    m_spriteImage.setPixelColor(x,y,m_currentColor);
                }
            }
        }
    } else if (m_currentTool == "Eraser"){
        for(int x = smallMousePoint.x(); x < m_brushAndEraserSize + smallMousePoint.x() && x < m_spriteSize.width(); x++){
            for(int y = smallMousePoint.y(); y < m_brushAndEraserSize + smallMousePoint.y() && y < m_spriteSize.height(); y++){
                m_spriteImage.setPixelColor(x,y,QColorConstants::Transparent);
                m_frames.replace(m_currentFrameIndex, m_spriteImage);
            }
        }
    }else {
        for(int x = smallMousePoint.x(); x < m_brushAndEraserSize + smallMousePoint.x() && x < m_spriteSize.width(); x++){
            for(int y = smallMousePoint.y(); y < m_brushAndEraserSize + smallMousePoint.y() && y < m_spriteSize.height(); y++){
                m_spriteImage.setPixelColor(x,y,m_currentColor);
                m_frames.replace(m_currentFrameIndex, m_spriteImage);
            }
        }
    }
    m_scaledImage = m_frames.at(m_currentFrameIndex).scaled(m_spriteImage.width() * m_zoomScale, m_spriteImage.height() * m_zoomScale, Qt::IgnoreAspectRatio, Qt::FastTransformation);
    m_lastMousePoint = mousePoint;
    m_unsaved = true;
    update();
}

///
/// \brief Helper method for the bucket tool that uses the flood fill algorithm.
/// \param x = Mouse point's x position
/// \param y = Mouse point's y position
///
void Canvas::bucketTool(int x, int y){
    QPoint newPoint = QPoint(x,y);
    bool negativeBounds = (newPoint.x() < 0) || (newPoint.y() < 0);
    bool overFlowBounds = (newPoint.x() > m_spriteSize.width() - 1 || newPoint.y() > m_spriteSize.height() - 1);
    if(negativeBounds || overFlowBounds || m_spriteImage.pixelColor(newPoint) == m_currentColor){
        return;
    }
    if(m_spriteImage.pixelColor(newPoint) != m_colorToReplace){
        return;
    }
    m_spriteImage.setPixelColor(newPoint, m_currentColor);
    m_frames.replace(m_currentFrameIndex, m_spriteImage);
    bucketTool(x + 1, y);
    bucketTool(x - 1, y);
    bucketTool(x, y + 1);
    bucketTool(x, y - 1);
}

///
/// \brief Creates a default gray checkered background when a new frame
/// is added.
///
void Canvas::setDefaultBackground(){
    m_defaultImage = QImage(m_spriteSize, QImage::Format_ARGB32);
    QColor darkGray(192, 192, 192);
    QColor lightGray(224, 224, 224);
    for(int i = 0; i < m_spriteSize.width(); i++){
        for(int j = 0; j < m_spriteSize.height(); j++){
            if(i % 2 == 0){
                if(j % 2 == 0){
                    m_defaultImage.setPixelColor(i, j, darkGray);
                }
                else{
                    m_defaultImage.setPixelColor(i, j, lightGray);
                }
            }
            else{
                if(j % 2 == 0){
                    m_defaultImage.setPixelColor(i, j, lightGray);
                }
                else{
                    m_defaultImage.setPixelColor(i, j, darkGray);
                }
            }
        }
    }
}

///
/// \brief Helper method to save the current sprite image vector into a JSON formatted
/// .ssp file. Loops through every row and column of each image and adds the rgba value
/// of every pixel into a 2D array that makes an image. Adds the frames, height, width,
/// and number of frames to each file.
///
void Canvas::saveProject(QFile &file) {
    QJsonObject project;
    // Store frame data
    QJsonArray jsonFrames;
    // loop through m_frames
    for (const QImage &frame : m_frames) {
        QJsonArray frameRows;
        // loop through rowPixels in frame
        for (int rowIndex = 0; rowIndex < frame.height(); ++rowIndex) {
            QJsonArray rowPixels;
            // loop through all pixels in that row
            for (int columnIndex = 0; columnIndex < frame.width(); ++columnIndex) {
                // Get the color
                const QColor color(frame.pixelColor(columnIndex, rowIndex));
                // if the color has transparency
                bool hasTransparency = color.alpha() != 255;
                QJsonObject colorInfo{
                    {"r", color.red()},
                    {"g", color.green()},
                    {"b", color.blue()}
                };
                // Add the alpha value
                if (hasTransparency) {
                    colorInfo["a"] = color.alpha();
                }
                rowPixels.append(colorInfo);
            }
            // Add the pixels in row
            frameRows.append(rowPixels);
        }
        // Add the frame
        jsonFrames.append(frameRows);
    }
    // Add the m_frames data
    project["m_frames"] = jsonFrames;
    project["height"] = m_spriteSize.height();
    project["width"] = m_spriteSize.width();
    project["numOfm_frames"] = m_frames.size();
    // Write the JSON data
    QJsonDocument jsonDoc(project);
    file.write(jsonDoc.toJson());
}

///
/// \brief Helper method to load a sprite image vector from a JSON formatted array.
///
/// \param file = .ssp file to load onto drawing canvas
///
void Canvas::loadProject(QFile &file)
{
    // Read the JSON data use readAll
    QByteArray jsonData = file.readAll();
    QJsonDocument jsonDoc(QJsonDocument::fromJson(jsonData));
    QJsonObject project = jsonDoc.object();
    // Get height, width, and numOfm_frames
    int height = project["height"].toInt();
    int width = project["width"].toInt();
    int numOfm_frames = project["numOfm_frames"].toInt();
    // Set m_spriteSize and clear m_frames
    m_spriteSize = QSize(width, height);
    m_frames.clear();
    // Get the m_frames data
    QJsonArray jsonFrames = project["m_frames"].toArray();
    // loop through m_frames
    for (const QJsonValue &frameValue : jsonFrames) {
        QJsonArray frameRows = frameValue.toArray();
        QImage frame(width, height, QImage::Format_ARGB32);
        // loop through rowPixels in frame
        int rowIndex = 0;
        for (const QJsonValue &rowValue : frameRows) {
            QJsonArray rowPixels = rowValue.toArray();
            // loop through all pixels in that row
            int columnIndex = 0;
            for (const QJsonValue &pixelValue : rowPixels) {
                QJsonObject colorInfo = pixelValue.toObject();
                // Get the color value
                int r = colorInfo["r"].toInt();
                int g = colorInfo["g"].toInt();
                int b = colorInfo["b"].toInt();
                int a = colorInfo.contains("a") ? colorInfo["a"].toInt() : 255;
                // Set the color
                frame.setPixelColor(columnIndex, rowIndex, QColor(r, g, b, a));
                columnIndex++;
            }
            rowIndex++;
        }
        // Add the frame
        m_frames.append(frame);
    }
    // Update the canvas
    m_imageScale = 512 / m_spriteSize.width();
    m_zoomScale = m_imageScale;
    setDefaultBackground();
    m_currentFrameIndex = m_frames.size() - 1;
    m_spriteImage = m_frames.at(m_currentFrameIndex).copy();
    copyAndScaleImage();
    update();
    emit updateFrameNumber(numOfm_frames - 1);
    // Check if there are previous m_frames
    if (m_currentFrameIndex > 0) {
        emit enableLastButton();
    } else {
        emit disableLastButton();
    }
    // Check if there are next m_frames
    if (m_currentFrameIndex < m_frames.size() - 1) {
        emit enableNextButton();
    } else {
        emit disableNextButton();
    }
}

///
/// \brief Helper method to scale the current image to size.
///
void Canvas::copyAndScaleImage(){
    m_scaledImage = m_spriteImage.copy();
    m_scaledImage = m_scaledImage.scaled(m_spriteImage.width() * m_zoomScale, m_spriteImage.height() * m_zoomScale, Qt::KeepAspectRatio, Qt::FastTransformation);
}

///
/// \brief Helper method to scale the default background image to size.
///
void Canvas::copyAndScaleDefaultImage(){
    m_scaledDefaultBackground = m_defaultImage.copy();
    m_scaledDefaultBackground = m_scaledDefaultBackground.scaled(m_spriteImage.width() * m_zoomScale, m_spriteImage.height() * m_zoomScale, Qt::KeepAspectRatio, Qt::FastTransformation);
}

///
/// \brief Adds a frame to the sprite image vector. Initializes with a default
/// background.
///
void Canvas::on_addFrameClicked(){
    m_spriteImage = QImage(m_spriteSize, QImage::Format_ARGB32);
    m_spriteImage.fill(QColorConstants::Transparent);
    if(m_currentFrameIndex == m_frames.size() - 1){
        m_frames.append(m_spriteImage);
        m_currentFrameIndex++;
        emit disableNextButton();
    }
    else{
        m_currentFrameIndex++;
        m_frames.insert(m_currentFrameIndex, m_spriteImage);
    }
    copyAndScaleImage();
    copyAndScaleDefaultImage();
    update();
    emit enableLastButton();
    emit enableDeleteButton();
    emit updateFrameNumber(m_currentFrameIndex);
}

///
/// \brief Deletes this frame from the sprite image vector.
/// Replaces current image with next available image based on vector
/// deletion convention.
///
void Canvas::on_deleteCurrentFrameClicked(){
    if(m_currentFrameIndex == m_frames.size() - 1){
        m_frames.removeAt(m_currentFrameIndex);
        m_currentFrameIndex--;
    }
    else {
        m_frames.removeAt(m_currentFrameIndex);
        if(m_currentFrameIndex == m_frames.size() - 1){
            emit disableNextButton();
        }
        else{
            emit enableNextButton();
        }
    }
    if (m_frames.size() == 1){
        emit disableNextButton();
        emit disableDeleteButton();
    }
    if(m_currentFrameIndex != 0){
        emit enableLastButton();
    }
    else{
        emit disableLastButton();
    }
    m_spriteImage = m_frames.at(m_currentFrameIndex).copy();
    copyAndScaleImage();
    update();
    emit updateFrameNumber(m_currentFrameIndex);
}

///
/// \brief Sets the current frame to the previous image in the sprite image vector.
///
void Canvas::on_lastFrameClicked(){
    m_currentFrameIndex--;
    m_spriteImage = m_frames.at(m_currentFrameIndex).copy();
    copyAndScaleImage();
    update();
    if(m_currentFrameIndex == 0){
        emit disableLastButton();
    }
    emit enableNextButton();
    emit updateFrameNumber(m_currentFrameIndex);
}

///
/// \brief Sets the current frame to the next image in the sprite image vector.
///
void Canvas::on_nextFrameClicked(){
    m_currentFrameIndex++;
    m_spriteImage = m_frames.at(m_currentFrameIndex).copy();
    copyAndScaleImage();
    update();
    emit enableLastButton();
    if(m_currentFrameIndex != m_frames.size() - 1){
        emit enableNextButton();
    }
    else {
        emit disableNextButton();
    }
    emit updateFrameNumber(m_currentFrameIndex);
}

///
/// \brief Creates a duplicate frame and adds it to the sprite image vector.
///
void Canvas::on_duplicateFrameClicked(){
    if(m_currentFrameIndex == m_frames.size() - 1){
        m_frames.append(m_spriteImage);
        m_currentFrameIndex++;
        emit disableNextButton();
    }
    else{
        m_currentFrameIndex++;
        m_frames.insert(m_currentFrameIndex, m_spriteImage);
    }
    copyAndScaleImage();
    update();
    emit enableLastButton();
    emit enableDeleteButton();
    emit updateFrameNumber(m_currentFrameIndex);
}

///
/// \brief Clears the current sprite image.
///
void Canvas::on_clearFrameClicked(){
      m_spriteImage.fill(QColorConstants::Transparent);
      m_frames.replace(m_currentFrameIndex, m_spriteImage);
}

///
/// \brief Clears any current sprite images and sets the new
/// sprite image size as chosen.
///
void Canvas::on_setSpriteSizeClicked() {
    bool Done;
    int size = QInputDialog::getInt(this,"Sprite size:", "WARNING: any unsaved data will be lost.\nEnter a size and click ok to create a new sprite. Use powers of two for best results",
                                    m_spriteSize.width(), 2, 128, 1, &Done);
    if (Done) {
        m_frames.clear();
        m_spriteSize = (QSize(size, size));
        m_spriteImage = QImage(m_spriteSize, QImage::Format_ARGB32);
        m_spriteImage.fill(QColorConstants::Transparent);
        m_frames.append(m_spriteImage);
        setDefaultBackground();
        m_imageScale = 512 / m_spriteSize.width();
        m_zoomScale = m_imageScale;
        m_currentColor = Qt::black;
        m_brushAndEraserSize = 1;
        m_currentFrameIndex = 0;
        copyAndScaleImage();
        copyAndScaleDefaultImage();
        update();
        emit disableNextButton();
        emit disableLastButton();
        emit disableDeleteButton();
        emit updateFrameNumber(m_currentFrameIndex);
    }
}

///
/// \brief Cues the file to be saved in an .ssp format to whatever
/// file path the user chooses.
///
void Canvas::on_SaveClicked() {
    QString fileName = QFileDialog::getSaveFileName(this, "Save Project", "", "Sprite Sheet Project (*.ssp)");
    if (!fileName.isEmpty()) {
        QFile file(fileName);
        if (file.open(QIODevice::WriteOnly)) {
            saveProject(file);
            file.close();
        } else {
            QMessageBox::warning(this, "Unable to save project", file.errorString());
        }
    }
}

///
/// \brief Cues the file to be loaded from an .ssp file into a modifiable
/// sprite vector that appears on the drawing canvas.
///
void Canvas::on_LoadClicked() {
    QString fileName = QFileDialog::getOpenFileName(this, "Loading", "", "Sprite Sheet Project (*.ssp)");
    if (!fileName.isEmpty()) {
        QFile file(fileName);
        if (file.open(QIODevice::ReadOnly)) {
            loadProject(file);
            file.close();
        } else {
            QMessageBox::warning(this, "Unable to load!", file.errorString());
        }
    }
}

///
/// \brief Starts the preview animation.
///
void Canvas::on_playButtonClicked(){
    emit updatePreview(m_frames, m_currentFrameIndex);
}

///
/// \brief Sets the brush and eraser size to 1.
///
void Canvas::setBrushAndEraserSize1(){
    m_brushAndEraserSize = 1;

}

///
/// \brief Sets the brush and eraser size to 2.
///
void Canvas::setBrushAndEraserSize2(){
    m_brushAndEraserSize = 2;

}

///
/// \brief Sets the brush and eraser size to 3.
///
void Canvas::setBrushAndEraserSize3(){
    m_brushAndEraserSize = 3;
}

///
/// \brief Sets the brush and eraser size to 4.
///
void Canvas::setBrushAndEraserSize4(){
    m_brushAndEraserSize = 4;
}

///
/// \brief Sets the current tool to brush.
///
void Canvas::setBrush(){
    m_currentTool = "Brush";
    m_brushAndEraserSize = 1;
}

///
/// \brief Sets the current tool to bucket.
///
void Canvas::setBucket(){
    m_currentTool = "Bucket";
}

///
/// \brief Sets the current tool to eraser.
///
void Canvas::setEraser(){
    m_brushAndEraserSize = 1;
    m_currentTool = "Eraser";
}

///
/// \brief Sets the current tool to tile.
///
void Canvas::setTile(){
    m_currentTool = "Tile";
}

///
/// \brief Sets the current color as chosen by the user.
///
void Canvas::setColor(){
    bool isColorSelected;
    QColor newColor = QColorDialog::getColor(Qt::black, this, NULL, QColorDialog::ShowAlphaChannel);
    if(&isColorSelected && newColor.isValid()){
        m_currentColor = newColor;
        QString color = "QPushButton{background-color: rgba(" + QString::fromStdString(std::to_string(newColor.red())) + "," +
                QString::fromStdString(std::to_string(newColor.green())) + "," +
                QString::fromStdString(std::to_string(newColor.blue())) + "," + QString::fromStdString(std::to_string(newColor.alpha())) + ");}";
        emit changeColorButton(color);
    }
}

///
/// \brief Zooms the drawing canvas in.
///
void Canvas::zoomIn(){
    int overFlowCheck = m_zoomScale * 2;
    if(!(overFlowCheck > m_imageScale)){
        m_zoomScale *= 2;
    }
    update();
}

///
/// \brief Zooms the drawing canvas out.
///
void Canvas::zoomOut(){
    int underFlowCheck = m_zoomScale / 2;
    if(!(underFlowCheck < 1)){
        m_zoomScale /= 2;
    }
    update();
}
