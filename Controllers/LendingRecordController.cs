using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using HttpEx.REST;
using Library.DataTransferObjects;
using Library.DomainModel;

namespace Library.WebApi
{
    public class LendingRecordController : ApiController
    {
        private ILendingService LendingService { get; set; }
        private LendingRecordResourceAssembler RecordAssembler { get; set; }

        public LendingRecordController( ILendingService lendingService, LendingRecordResourceAssembler assembler )
        {
            LendingService = lendingService;
            RecordAssembler = assembler;
        }

        /// <summary>
        /// Gets all the lending records.
        /// </summary>
        [HttpGet]
        [ResponseType( typeof( ResourceCollection<LendingRecordResource> ) )]
        public async Task<IHttpActionResult> GetAsync( string expand = null )
        {
            IEnumerable<LendingRecord> records = await LendingService.GetAllRecordsAsync();
            var resourceCollection = await RecordAssembler.ConvertToResourceCollectionAsync( this, records, expand );
            return Ok( resourceCollection );
        }

        /// <summary>
        /// Retrieves the lending record with the specified id.
        /// </summary>
        /// <param name="id">The id of the lending record to retrieve.</param>
        [HttpGet]
        [ResponseType( typeof( LendingRecordResource ) )]
        public async Task<IHttpActionResult> GetAsync( string id, string expand = null )
        {
            var record = await LendingService.GetByIdAsync( id );
            if( record == null ) return NotFound();

            var resource = await RecordAssembler.ConvertToResourceAsync( this, record, expand );
            return Ok( resource );
        }
    }
}
