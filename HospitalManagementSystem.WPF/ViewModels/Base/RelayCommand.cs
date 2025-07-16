//using System;
//using System.Windows.Input; // Required for ICommand and CommandManager

//namespace HospitalManagementSystem.WPF.ViewModels.Base
//{
//    public class RelayCommand : ICommand
//    {
//        private readonly Action<object> _execute; //designed to store the method (or lambda expression)
//        private readonly Predicate<object> _canExecute;

//        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
//        {
//            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
//            _canExecute = canExecute;
//        }

//        // Modified: Explicit CanExecuteChanged event and public RaiseCanExecuteChanged method
//        // Changed from `add { CommandManager.RequerySuggested += value; }`
//        // to a more traditional event pattern so we can raise it manually.
//        public event EventHandler CanExecuteChanged;

//        /// <summary>
//        /// Raises the <see cref="CanExecuteChanged"/> event, signaling that the command's
//        /// ability to execute should be re-evaluated by the command system.
//        /// </summary>
//        public void RaiseCanExecuteChanged()
//        {
//            // The null-conditional operator '?.' ensures that the event is only invoked if there are subscribers.
//            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
//        }

//        public bool CanExecute(object parameter)
//        {
//            return _canExecute == null || _canExecute(parameter);
//        }

//        public void Execute(object parameter)
//        {
//            _execute(parameter);
//        }
//    }
//}