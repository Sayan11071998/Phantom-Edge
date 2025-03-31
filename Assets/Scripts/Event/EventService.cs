namespace StatePattern.Events
{
    public class EventService
    {
        public EventController<int> OnLevelSelected { get; private set; }

        public EventService()
        {
            OnLevelSelected = new EventController<int>();
        }
    }
}