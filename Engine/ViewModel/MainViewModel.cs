using System;
using Engine.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Engine.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand SendCommand { get; private set; }


        private string _message = string.Empty;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (!_message.Equals(value))
                {
                    _message = value;
                    RaisePropertyChanged(nameof(Message));
                    SendCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string _text = string.Empty;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (!_text.Equals(value))
                {
                    _text = value;
                    RaisePropertyChanged(nameof(Text));
                    SendCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private readonly IMessageProducer _messageProducer;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer;
            SendCommand = new RelayCommand(DoSendCommand, CanDoSendCommand);
            Message = "Hello world!";
            Text = string.Empty;
        }

        private void DoSendCommand()
        {
            //MessageProducer.Instance.Send();
            _messageProducer.Send();
        }

        private bool CanDoSendCommand()
        {
            return Text.Equals("1");
        }
    }
}