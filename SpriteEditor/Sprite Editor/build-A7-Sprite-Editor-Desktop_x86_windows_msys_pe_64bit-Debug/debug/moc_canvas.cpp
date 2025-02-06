/****************************************************************************
** Meta object code from reading C++ file 'canvas.h'
**
** Created by: The Qt Meta Object Compiler version 68 (Qt 6.4.3)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include <memory>
#include "../../A7-Sprite-Editor/canvas.h"
#include <QtGui/qtextcursor.h>
#include <QtCore/qmetatype.h>
#include <QtCore/QList>
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'canvas.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 68
#error "This file was generated using the moc from 6.4.3. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

#ifndef Q_CONSTINIT
#define Q_CONSTINIT
#endif

QT_BEGIN_MOC_NAMESPACE
QT_WARNING_PUSH
QT_WARNING_DISABLE_DEPRECATED
namespace {
struct qt_meta_stringdata_Canvas_t {
    uint offsetsAndSizes[74];
    char stringdata0[7];
    char stringdata1[14];
    char stringdata2[1];
    char stringdata3[14];
    char stringdata4[7];
    char stringdata5[6];
    char stringdata6[18];
    char stringdata7[6];
    char stringdata8[18];
    char stringdata9[9];
    char stringdata10[17];
    char stringdata11[18];
    char stringdata12[17];
    char stringdata13[18];
    char stringdata14[19];
    char stringdata15[20];
    char stringdata16[19];
    char stringdata17[29];
    char stringdata18[20];
    char stringdata19[20];
    char stringdata20[25];
    char stringdata21[21];
    char stringdata22[24];
    char stringdata23[15];
    char stringdata24[15];
    char stringdata25[21];
    char stringdata26[23];
    char stringdata27[23];
    char stringdata28[23];
    char stringdata29[23];
    char stringdata30[9];
    char stringdata31[10];
    char stringdata32[10];
    char stringdata33[8];
    char stringdata34[9];
    char stringdata35[7];
    char stringdata36[8];
};
#define QT_MOC_LITERAL(ofs, len) \
    uint(sizeof(qt_meta_stringdata_Canvas_t::offsetsAndSizes) + ofs), len 
Q_CONSTINIT static const qt_meta_stringdata_Canvas_t qt_meta_stringdata_Canvas = {
    {
        QT_MOC_LITERAL(0, 6),  // "Canvas"
        QT_MOC_LITERAL(7, 13),  // "updatePreview"
        QT_MOC_LITERAL(21, 0),  // ""
        QT_MOC_LITERAL(22, 13),  // "QList<QImage>"
        QT_MOC_LITERAL(36, 6),  // "frames"
        QT_MOC_LITERAL(43, 5),  // "index"
        QT_MOC_LITERAL(49, 17),  // "changeColorButton"
        QT_MOC_LITERAL(67, 5),  // "color"
        QT_MOC_LITERAL(73, 17),  // "updateFrameNumber"
        QT_MOC_LITERAL(91, 8),  // "frameNum"
        QT_MOC_LITERAL(100, 16),  // "enableLastButton"
        QT_MOC_LITERAL(117, 17),  // "disableLastButton"
        QT_MOC_LITERAL(135, 16),  // "enableNextButton"
        QT_MOC_LITERAL(152, 17),  // "disableNextButton"
        QT_MOC_LITERAL(170, 18),  // "enableDeleteButton"
        QT_MOC_LITERAL(189, 19),  // "disableDeleteButton"
        QT_MOC_LITERAL(209, 18),  // "on_addFrameClicked"
        QT_MOC_LITERAL(228, 28),  // "on_deleteCurrentFrameClicked"
        QT_MOC_LITERAL(257, 19),  // "on_lastFrameClicked"
        QT_MOC_LITERAL(277, 19),  // "on_nextFrameClicked"
        QT_MOC_LITERAL(297, 24),  // "on_duplicateFrameClicked"
        QT_MOC_LITERAL(322, 20),  // "on_clearFrameClicked"
        QT_MOC_LITERAL(343, 23),  // "on_setSpriteSizeClicked"
        QT_MOC_LITERAL(367, 14),  // "on_SaveClicked"
        QT_MOC_LITERAL(382, 14),  // "on_LoadClicked"
        QT_MOC_LITERAL(397, 20),  // "on_playButtonClicked"
        QT_MOC_LITERAL(418, 22),  // "setBrushAndEraserSize1"
        QT_MOC_LITERAL(441, 22),  // "setBrushAndEraserSize2"
        QT_MOC_LITERAL(464, 22),  // "setBrushAndEraserSize3"
        QT_MOC_LITERAL(487, 22),  // "setBrushAndEraserSize4"
        QT_MOC_LITERAL(510, 8),  // "setBrush"
        QT_MOC_LITERAL(519, 9),  // "setBucket"
        QT_MOC_LITERAL(529, 9),  // "setEraser"
        QT_MOC_LITERAL(539, 7),  // "setTile"
        QT_MOC_LITERAL(547, 8),  // "setColor"
        QT_MOC_LITERAL(556, 6),  // "zoomIn"
        QT_MOC_LITERAL(563, 7)   // "zoomOut"
    },
    "Canvas",
    "updatePreview",
    "",
    "QList<QImage>",
    "frames",
    "index",
    "changeColorButton",
    "color",
    "updateFrameNumber",
    "frameNum",
    "enableLastButton",
    "disableLastButton",
    "enableNextButton",
    "disableNextButton",
    "enableDeleteButton",
    "disableDeleteButton",
    "on_addFrameClicked",
    "on_deleteCurrentFrameClicked",
    "on_lastFrameClicked",
    "on_nextFrameClicked",
    "on_duplicateFrameClicked",
    "on_clearFrameClicked",
    "on_setSpriteSizeClicked",
    "on_SaveClicked",
    "on_LoadClicked",
    "on_playButtonClicked",
    "setBrushAndEraserSize1",
    "setBrushAndEraserSize2",
    "setBrushAndEraserSize3",
    "setBrushAndEraserSize4",
    "setBrush",
    "setBucket",
    "setEraser",
    "setTile",
    "setColor",
    "zoomIn",
    "zoomOut"
};
#undef QT_MOC_LITERAL
} // unnamed namespace

Q_CONSTINIT static const uint qt_meta_data_Canvas[] = {

 // content:
      10,       // revision
       0,       // classname
       0,    0, // classinfo
      30,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       9,       // signalCount

 // signals: name, argc, parameters, tag, flags, initial metatype offsets
       1,    2,  194,    2, 0x06,    1 /* Public */,
       6,    1,  199,    2, 0x06,    4 /* Public */,
       8,    1,  202,    2, 0x06,    6 /* Public */,
      10,    0,  205,    2, 0x06,    8 /* Public */,
      11,    0,  206,    2, 0x06,    9 /* Public */,
      12,    0,  207,    2, 0x06,   10 /* Public */,
      13,    0,  208,    2, 0x06,   11 /* Public */,
      14,    0,  209,    2, 0x06,   12 /* Public */,
      15,    0,  210,    2, 0x06,   13 /* Public */,

 // slots: name, argc, parameters, tag, flags, initial metatype offsets
      16,    0,  211,    2, 0x0a,   14 /* Public */,
      17,    0,  212,    2, 0x0a,   15 /* Public */,
      18,    0,  213,    2, 0x0a,   16 /* Public */,
      19,    0,  214,    2, 0x0a,   17 /* Public */,
      20,    0,  215,    2, 0x0a,   18 /* Public */,
      21,    0,  216,    2, 0x0a,   19 /* Public */,
      22,    0,  217,    2, 0x0a,   20 /* Public */,
      23,    0,  218,    2, 0x0a,   21 /* Public */,
      24,    0,  219,    2, 0x0a,   22 /* Public */,
      25,    0,  220,    2, 0x0a,   23 /* Public */,
      26,    0,  221,    2, 0x0a,   24 /* Public */,
      27,    0,  222,    2, 0x0a,   25 /* Public */,
      28,    0,  223,    2, 0x0a,   26 /* Public */,
      29,    0,  224,    2, 0x0a,   27 /* Public */,
      30,    0,  225,    2, 0x0a,   28 /* Public */,
      31,    0,  226,    2, 0x0a,   29 /* Public */,
      32,    0,  227,    2, 0x0a,   30 /* Public */,
      33,    0,  228,    2, 0x0a,   31 /* Public */,
      34,    0,  229,    2, 0x0a,   32 /* Public */,
      35,    0,  230,    2, 0x0a,   33 /* Public */,
      36,    0,  231,    2, 0x0a,   34 /* Public */,

 // signals: parameters
    QMetaType::Void, 0x80000000 | 3, QMetaType::Int,    4,    5,
    QMetaType::Void, QMetaType::QString,    7,
    QMetaType::Void, QMetaType::Int,    9,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,

 // slots: parameters
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,

       0        // eod
};

Q_CONSTINIT const QMetaObject Canvas::staticMetaObject = { {
    QMetaObject::SuperData::link<QWidget::staticMetaObject>(),
    qt_meta_stringdata_Canvas.offsetsAndSizes,
    qt_meta_data_Canvas,
    qt_static_metacall,
    nullptr,
    qt_incomplete_metaTypeArray<qt_meta_stringdata_Canvas_t,
        // Q_OBJECT / Q_GADGET
        QtPrivate::TypeAndForceComplete<Canvas, std::true_type>,
        // method 'updatePreview'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        QtPrivate::TypeAndForceComplete<QVector<QImage>, std::false_type>,
        QtPrivate::TypeAndForceComplete<int, std::false_type>,
        // method 'changeColorButton'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        QtPrivate::TypeAndForceComplete<QString, std::false_type>,
        // method 'updateFrameNumber'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        QtPrivate::TypeAndForceComplete<int, std::false_type>,
        // method 'enableLastButton'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'disableLastButton'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'enableNextButton'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'disableNextButton'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'enableDeleteButton'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'disableDeleteButton'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'on_addFrameClicked'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'on_deleteCurrentFrameClicked'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'on_lastFrameClicked'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'on_nextFrameClicked'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'on_duplicateFrameClicked'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'on_clearFrameClicked'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'on_setSpriteSizeClicked'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'on_SaveClicked'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'on_LoadClicked'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'on_playButtonClicked'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'setBrushAndEraserSize1'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'setBrushAndEraserSize2'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'setBrushAndEraserSize3'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'setBrushAndEraserSize4'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'setBrush'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'setBucket'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'setEraser'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'setTile'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'setColor'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'zoomIn'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'zoomOut'
        QtPrivate::TypeAndForceComplete<void, std::false_type>
    >,
    nullptr
} };

void Canvas::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        auto *_t = static_cast<Canvas *>(_o);
        (void)_t;
        switch (_id) {
        case 0: _t->updatePreview((*reinterpret_cast< std::add_pointer_t<QList<QImage>>>(_a[1])),(*reinterpret_cast< std::add_pointer_t<int>>(_a[2]))); break;
        case 1: _t->changeColorButton((*reinterpret_cast< std::add_pointer_t<QString>>(_a[1]))); break;
        case 2: _t->updateFrameNumber((*reinterpret_cast< std::add_pointer_t<int>>(_a[1]))); break;
        case 3: _t->enableLastButton(); break;
        case 4: _t->disableLastButton(); break;
        case 5: _t->enableNextButton(); break;
        case 6: _t->disableNextButton(); break;
        case 7: _t->enableDeleteButton(); break;
        case 8: _t->disableDeleteButton(); break;
        case 9: _t->on_addFrameClicked(); break;
        case 10: _t->on_deleteCurrentFrameClicked(); break;
        case 11: _t->on_lastFrameClicked(); break;
        case 12: _t->on_nextFrameClicked(); break;
        case 13: _t->on_duplicateFrameClicked(); break;
        case 14: _t->on_clearFrameClicked(); break;
        case 15: _t->on_setSpriteSizeClicked(); break;
        case 16: _t->on_SaveClicked(); break;
        case 17: _t->on_LoadClicked(); break;
        case 18: _t->on_playButtonClicked(); break;
        case 19: _t->setBrushAndEraserSize1(); break;
        case 20: _t->setBrushAndEraserSize2(); break;
        case 21: _t->setBrushAndEraserSize3(); break;
        case 22: _t->setBrushAndEraserSize4(); break;
        case 23: _t->setBrush(); break;
        case 24: _t->setBucket(); break;
        case 25: _t->setEraser(); break;
        case 26: _t->setTile(); break;
        case 27: _t->setColor(); break;
        case 28: _t->zoomIn(); break;
        case 29: _t->zoomOut(); break;
        default: ;
        }
    } else if (_c == QMetaObject::RegisterMethodArgumentMetaType) {
        switch (_id) {
        default: *reinterpret_cast<QMetaType *>(_a[0]) = QMetaType(); break;
        case 0:
            switch (*reinterpret_cast<int*>(_a[1])) {
            default: *reinterpret_cast<QMetaType *>(_a[0]) = QMetaType(); break;
            case 0:
                *reinterpret_cast<QMetaType *>(_a[0]) = QMetaType::fromType< QList<QImage> >(); break;
            }
            break;
        }
    } else if (_c == QMetaObject::IndexOfMethod) {
        int *result = reinterpret_cast<int *>(_a[0]);
        {
            using _t = void (Canvas::*)(QVector<QImage> , int );
            if (_t _q_method = &Canvas::updatePreview; *reinterpret_cast<_t *>(_a[1]) == _q_method) {
                *result = 0;
                return;
            }
        }
        {
            using _t = void (Canvas::*)(QString );
            if (_t _q_method = &Canvas::changeColorButton; *reinterpret_cast<_t *>(_a[1]) == _q_method) {
                *result = 1;
                return;
            }
        }
        {
            using _t = void (Canvas::*)(int );
            if (_t _q_method = &Canvas::updateFrameNumber; *reinterpret_cast<_t *>(_a[1]) == _q_method) {
                *result = 2;
                return;
            }
        }
        {
            using _t = void (Canvas::*)();
            if (_t _q_method = &Canvas::enableLastButton; *reinterpret_cast<_t *>(_a[1]) == _q_method) {
                *result = 3;
                return;
            }
        }
        {
            using _t = void (Canvas::*)();
            if (_t _q_method = &Canvas::disableLastButton; *reinterpret_cast<_t *>(_a[1]) == _q_method) {
                *result = 4;
                return;
            }
        }
        {
            using _t = void (Canvas::*)();
            if (_t _q_method = &Canvas::enableNextButton; *reinterpret_cast<_t *>(_a[1]) == _q_method) {
                *result = 5;
                return;
            }
        }
        {
            using _t = void (Canvas::*)();
            if (_t _q_method = &Canvas::disableNextButton; *reinterpret_cast<_t *>(_a[1]) == _q_method) {
                *result = 6;
                return;
            }
        }
        {
            using _t = void (Canvas::*)();
            if (_t _q_method = &Canvas::enableDeleteButton; *reinterpret_cast<_t *>(_a[1]) == _q_method) {
                *result = 7;
                return;
            }
        }
        {
            using _t = void (Canvas::*)();
            if (_t _q_method = &Canvas::disableDeleteButton; *reinterpret_cast<_t *>(_a[1]) == _q_method) {
                *result = 8;
                return;
            }
        }
    }
}

const QMetaObject *Canvas::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->dynamicMetaObject() : &staticMetaObject;
}

void *Canvas::qt_metacast(const char *_clname)
{
    if (!_clname) return nullptr;
    if (!strcmp(_clname, qt_meta_stringdata_Canvas.stringdata0))
        return static_cast<void*>(this);
    return QWidget::qt_metacast(_clname);
}

int Canvas::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QWidget::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 30)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 30;
    } else if (_c == QMetaObject::RegisterMethodArgumentMetaType) {
        if (_id < 30)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 30;
    }
    return _id;
}

// SIGNAL 0
void Canvas::updatePreview(QVector<QImage> _t1, int _t2)
{
    void *_a[] = { nullptr, const_cast<void*>(reinterpret_cast<const void*>(std::addressof(_t1))), const_cast<void*>(reinterpret_cast<const void*>(std::addressof(_t2))) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}

// SIGNAL 1
void Canvas::changeColorButton(QString _t1)
{
    void *_a[] = { nullptr, const_cast<void*>(reinterpret_cast<const void*>(std::addressof(_t1))) };
    QMetaObject::activate(this, &staticMetaObject, 1, _a);
}

// SIGNAL 2
void Canvas::updateFrameNumber(int _t1)
{
    void *_a[] = { nullptr, const_cast<void*>(reinterpret_cast<const void*>(std::addressof(_t1))) };
    QMetaObject::activate(this, &staticMetaObject, 2, _a);
}

// SIGNAL 3
void Canvas::enableLastButton()
{
    QMetaObject::activate(this, &staticMetaObject, 3, nullptr);
}

// SIGNAL 4
void Canvas::disableLastButton()
{
    QMetaObject::activate(this, &staticMetaObject, 4, nullptr);
}

// SIGNAL 5
void Canvas::enableNextButton()
{
    QMetaObject::activate(this, &staticMetaObject, 5, nullptr);
}

// SIGNAL 6
void Canvas::disableNextButton()
{
    QMetaObject::activate(this, &staticMetaObject, 6, nullptr);
}

// SIGNAL 7
void Canvas::enableDeleteButton()
{
    QMetaObject::activate(this, &staticMetaObject, 7, nullptr);
}

// SIGNAL 8
void Canvas::disableDeleteButton()
{
    QMetaObject::activate(this, &staticMetaObject, 8, nullptr);
}
QT_WARNING_POP
QT_END_MOC_NAMESPACE
