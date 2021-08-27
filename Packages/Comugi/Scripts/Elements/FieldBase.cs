﻿namespace Comugi
{
    /// <summary>
    /// 値を持ち外部と同期するElement
    /// </summary>
    public abstract class FieldBase<T> : ReadOnlyField<T>
    {
       readonly BinderBase<T> binder;

        public FieldBase(LabelElement label, BinderBase<T> binder) : base(label, binder)
        {
            this.binder = binder;
            interactable = !binder.IsReadOnly;
        }


        #region Internal

        public void OnViewValueChanged(T t) => binder?.Set(t);

        #endregion
    }
}