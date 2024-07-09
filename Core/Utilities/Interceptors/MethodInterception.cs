using Castle.DynamicProxy;

namespace Core.Utilities.Interceptors
{
    //BİZİM KODLAR İÇERİSİNDE TRY CATCH YAZMAMA SEBEBİMİZ BU CLASS. BİZ BU CLASS ' DA TEMEL BİR TRY CATCH YAZDIK. NEDEN GİDİP
    //HER METODDA BU TRY CATCH'İ YAZAYDIM?  //BURASI BİZİM BÜTÜN METODLARIMIZIN ÇATISI!!  BİZİM BÜTÜN METODLARIMIZ BURADAKİ KURALLARDAN GEÇECEK.

    public abstract class MethodInterception : MethodInterceptionBaseAttribute
    {
        protected virtual void OnBefore(IInvocation invocation) { }
        protected virtual void OnAfter(IInvocation invocation) { }
        protected virtual void OnException(IInvocation invocation, System.Exception e) { }
        protected virtual void OnSuccess(IInvocation invocation) { }
        public override void Intercept(IInvocation invocation)  //Buradaki IINvocation invocation bizim için çalıştırmak istediğimiz metodumuz mesela Add() metodu olsun
        {
            var isSuccess = true;
            OnBefore(invocation); //İşte ben metodumu nerede çalıştırmak istersem onu belirtiriz mesela OnBefore = metodun başında çalışır Genellikle %90OnBefore ve OnException' ı kullanırız ama diğerleride kullanılabilir
            try
            {
                invocation.Proceed(); //Çalıştırmak istediğimiz metodun çalışmasını sağlar ve eğer metod başarıyla tamamlanırsa catch bloğu atlanır  doğrudan finally bloğuna geçilir
            }
            catch (Exception e) // invocation.Proceed() metodunun çalışması sırasında bir istisna (hata) fırlatılırsa bu blok çalışır.                                             
            { 
                isSuccess = false; // Burada isSuccess değişkeni false olarak ayarlanır ve OnException(invocation, e) metodu çağrılır.
                OnException(invocation, e);  //Eğer hata aldığında çalıştıor dersem .    // Bu metod, istisna durumunu ele almak için kullanılır. Ardından istisna yeniden fırlatılır (throw), böylece dışarıdaki kod bu istisnayı yakalayabilir.
                throw;
            }
            finally
            {
                //try veya catch blokları sonrasında her durumda çalışacak kod bulunur.
                //Eğer isSuccess değişkeni true ise (yani, istisna fırlatılmadıysa), OnSuccess(invocation) metodu çağrılır.
                //Bu metod, işlemin başarıyla tamamlandığını belirten kodları içerir.
                if (isSuccess)
                {
                    OnSuccess(invocation);  //Metod baaşarılı olduğunda çalıştır dersem   
                }
            }
            OnAfter(invocation);  //Metoddan sonra çalışsın istersem


            //Biz hangi metodu doldurursak o çalışacak OnBefore ' u doldurursam o OnAfter'ı doldurursam o çalışır. 
            //Gerçek hayatta %90 OnBefore ve OnException kullanılı. ama şunu unutma biz OnAftter içini doldurmasak ' da bu kodda çağrılır sadece içi boş olduğu için kullanılmaz
            //Bunların içlerini doldurduğmuz yer şurası => namespace Core.Aspects.Autofac.Validation.ValidationAspect

        }
    }
}
//Bu şekilde OnBefore ve OnAfter metodları her zaman çalışacaktır.
//OnException ve OnSuccess metodları ise sırasıyla hata durumunda ve başarı durumunda çalışacaktır.