using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Library.DataTransferObjects;
using Library.DomainModel;

namespace Library.WebApi
{
    [RoutePrefix( "Borrow" )]
    public class BorrowController : ApiController
    {
        public ILendingService LendingService { get; set; }
        private LendingRecordResourceAssembler RecordAssembler { get; set; }

        public BorrowController( ILendingService lendingService, LendingRecordResourceAssembler recordAssembler )
        {
            LendingService = lendingService;
            RecordAssembler = recordAssembler;
        }

        /// <summary>
        /// Checks out the book with the specified id.
        /// </summary>
        /// <param name="bookId">The id of the book.</param>
        [HttpPost, Route( "checkout/{bookId}", Name = "Borrow_Checkout" )]
        [ResponseType( typeof( LendingRecordResource ) )]
        public async Task<IHttpActionResult> CheckoutAsync( string bookId )
        {
            try
            {
                var lendingRecord = await LendingService.CheckoutAsync( bookId );
                var resource = await RecordAssembler.ConvertToResourceAsync( lendingRecord );

                return CreatedAtRoute( Startup.DefaultRouteName, new { controller = "lendingRecord", id = lendingRecord.Id }, resource );
            }
            catch( DomainOperationException ex )
            {
                return BadRequest( ex.Message );
            }
            catch( InvalidOperationException )
            {
                return BadRequest( "that book is already checked out" );
            }
        }

        /// <summary>
        /// Checks in the book with the specified id.
        /// </summary>
        /// <param name="bookId">The id of the book.</param>
        [HttpPost, Route( "checkin/{bookId}", Name = "Borrow_Checkin" )]
        [ResponseType( typeof( LendingRecordResource ) )]
        public async Task<IHttpActionResult> CheckinAsync( string bookId )
        {
            try
            {
                var lendingRecord = await LendingService.CheckinAsync( bookId );
                var resource = await RecordAssembler.ConvertToResourceAsync( lendingRecord );

                return Ok( resource );
            }
            catch( DomainOperationException ex )
            {
                return BadRequest( ex.Message );
            }
            catch( InvalidOperationException )
            {
                return BadRequest( "that book is already checked in" );
            }
        }
    }
}
