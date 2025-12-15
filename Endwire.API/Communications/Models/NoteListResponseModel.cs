namespace EndWire.API.Communications.Models
{
    public class NoteListResponseModel
    {
        public List<NoteModel> Notes { get; set; }
        public NoteListResponseModel(List<NoteModel> notes) {
            Notes = notes;
        }
    }

    public sealed class NoteModel
    {
        public NoteModel() { }
        public int Id { get; set; }
        public string NoteId { get; set; }
    }
}
