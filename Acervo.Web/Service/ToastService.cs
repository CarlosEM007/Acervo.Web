namespace Acervo.Web.Service
{
    public enum ToastLevel { Success, Error, Info }

    public class ToastMessage
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Text { get; init; } = string.Empty;
        public ToastLevel Level { get; init; }
    }

    /// <summary>
    /// Fila de notificações (toasts) por circuito. As telas chamam
    /// <see cref="ShowSuccess"/> / <see cref="ShowError"/> e o componente
    /// ToastContainer renderiza reagindo ao evento <see cref="OnChange"/>.
    /// </summary>
    public class ToastService
    {
        private readonly List<ToastMessage> _toasts = new();

        public IReadOnlyList<ToastMessage> Toasts => _toasts;

        public event Action? OnChange;

        public void ShowSuccess(string message) => Show(message, ToastLevel.Success);
        public void ShowError(string message)   => Show(message, ToastLevel.Error);
        public void ShowInfo(string message)    => Show(message, ToastLevel.Info);

        public void Show(string message, ToastLevel level, int durationMs = 4000)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            var toast = new ToastMessage { Text = message, Level = level };
            _toasts.Add(toast);
            OnChange?.Invoke();

            _ = AutoDismissAsync(toast.Id, durationMs);
        }

        public void Remove(Guid id)
        {
            var toast = _toasts.FirstOrDefault(t => t.Id == id);
            if (toast is null) return;

            _toasts.Remove(toast);
            OnChange?.Invoke();
        }

        private async Task AutoDismissAsync(Guid id, int durationMs)
        {
            await Task.Delay(durationMs);
            Remove(id);
        }
    }
}
