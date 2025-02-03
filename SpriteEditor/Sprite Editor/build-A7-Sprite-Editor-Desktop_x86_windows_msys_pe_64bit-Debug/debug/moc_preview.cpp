/****************************************************************************
** Meta object code from reading C++ file 'preview.h'
**
** Created by: The Qt Meta Object Compiler version 68 (Qt 6.4.3)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include <memory>
#include "../../A7-Sprite-Editor/preview.h"
#include <QtCore/qmetatype.h>
#include <QtCore/QList>
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'preview.h' doesn't include <QObject>."
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
struct qt_meta_stringdata_Preview_t {
    uint offsetsAndSizes[20];
    char stringdata0[8];
    char stringdata1[14];
    char stringdata2[1];
    char stringdata3[14];
    char stringdata4[6];
    char stringdata5[12];
    char stringdata6[16];
    char stringdata7[17];
    char stringdata8[10];
    char stringdata9[11];
};
#define QT_MOC_LITERAL(ofs, len) \
    uint(sizeof(qt_meta_stringdata_Preview_t::offsetsAndSizes) + ofs), len 
Q_CONSTINIT static const qt_meta_stringdata_Preview_t qt_meta_stringdata_Preview = {
    {
        QT_MOC_LITERAL(0, 7),  // "Preview"
        QT_MOC_LITERAL(8, 13),  // "updatePreview"
        QT_MOC_LITERAL(22, 0),  // ""
        QT_MOC_LITERAL(23, 13),  // "QList<QImage>"
        QT_MOC_LITERAL(37, 5),  // "index"
        QT_MOC_LITERAL(43, 11),  // "swapPreview"
        QT_MOC_LITERAL(55, 15),  // "setPlaybackTrue"
        QT_MOC_LITERAL(71, 16),  // "setPlaybackFalse"
        QT_MOC_LITERAL(88, 9),  // "changeFPS"
        QT_MOC_LITERAL(98, 10)   // "actualSize"
    },
    "Preview",
    "updatePreview",
    "",
    "QList<QImage>",
    "index",
    "swapPreview",
    "setPlaybackTrue",
    "setPlaybackFalse",
    "changeFPS",
    "actualSize"
};
#undef QT_MOC_LITERAL
} // unnamed namespace

Q_CONSTINIT static const uint qt_meta_data_Preview[] = {

 // content:
      10,       // revision
       0,       // classname
       0,    0, // classinfo
       6,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       0,       // signalCount

 // slots: name, argc, parameters, tag, flags, initial metatype offsets
       1,    2,   50,    2, 0x0a,    1 /* Public */,
       5,    0,   55,    2, 0x0a,    4 /* Public */,
       6,    0,   56,    2, 0x0a,    5 /* Public */,
       7,    0,   57,    2, 0x0a,    6 /* Public */,
       8,    1,   58,    2, 0x0a,    7 /* Public */,
       9,    0,   61,    2, 0x0a,    9 /* Public */,

 // slots: parameters
    QMetaType::Void, 0x80000000 | 3, QMetaType::Int,    2,    4,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void, QMetaType::Int,    4,
    QMetaType::Void,

       0        // eod
};

Q_CONSTINIT const QMetaObject Preview::staticMetaObject = { {
    QMetaObject::SuperData::link<QWidget::staticMetaObject>(),
    qt_meta_stringdata_Preview.offsetsAndSizes,
    qt_meta_data_Preview,
    qt_static_metacall,
    nullptr,
    qt_incomplete_metaTypeArray<qt_meta_stringdata_Preview_t,
        // Q_OBJECT / Q_GADGET
        QtPrivate::TypeAndForceComplete<Preview, std::true_type>,
        // method 'updatePreview'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        QtPrivate::TypeAndForceComplete<QVector<QImage>, std::false_type>,
        QtPrivate::TypeAndForceComplete<int, std::false_type>,
        // method 'swapPreview'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'setPlaybackTrue'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'setPlaybackFalse'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        // method 'changeFPS'
        QtPrivate::TypeAndForceComplete<void, std::false_type>,
        QtPrivate::TypeAndForceComplete<int, std::false_type>,
        // method 'actualSize'
        QtPrivate::TypeAndForceComplete<void, std::false_type>
    >,
    nullptr
} };

void Preview::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        auto *_t = static_cast<Preview *>(_o);
        (void)_t;
        switch (_id) {
        case 0: _t->updatePreview((*reinterpret_cast< std::add_pointer_t<QList<QImage>>>(_a[1])),(*reinterpret_cast< std::add_pointer_t<int>>(_a[2]))); break;
        case 1: _t->swapPreview(); break;
        case 2: _t->setPlaybackTrue(); break;
        case 3: _t->setPlaybackFalse(); break;
        case 4: _t->changeFPS((*reinterpret_cast< std::add_pointer_t<int>>(_a[1]))); break;
        case 5: _t->actualSize(); break;
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
    }
}

const QMetaObject *Preview::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->dynamicMetaObject() : &staticMetaObject;
}

void *Preview::qt_metacast(const char *_clname)
{
    if (!_clname) return nullptr;
    if (!strcmp(_clname, qt_meta_stringdata_Preview.stringdata0))
        return static_cast<void*>(this);
    return QWidget::qt_metacast(_clname);
}

int Preview::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QWidget::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 6)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 6;
    } else if (_c == QMetaObject::RegisterMethodArgumentMetaType) {
        if (_id < 6)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 6;
    }
    return _id;
}
QT_WARNING_POP
QT_END_MOC_NAMESPACE
