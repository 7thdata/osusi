namespace wppBacklog.Helpers
{
    public class MailHelpers
    {
        public static string PreRegistered(string culture, string name, string url)
        {
            var content = "";

            if (culture == "ja")
            {
                content += name + "様、<br/><br/>Backlog by SEVENTHに仮登録いただき、ありがとうございます。<br/><br/>";
                content += "下記のリンクをクリックしメールアドレス確認及び本登録とさせていただきます。<br/>";
                content += "<a href='" + url + "'>" + url + "</a><br/>";
                content += Footer(culture);

                return content;
            }

            content += "Hello " + name + ", <br/><br/>You are pre-registered to BACKLOG by SEVENTH<br/><br/>";
            content += "Please click on link below to confirm your registration.<br/>";
            content += "<a href='" + url + "'>" + url + "</a><br/>";
            content += Footer(culture);

            return content;
        }

        public static string Invited(string culture, string orgName, string name, string password, string url, string memo)
        {
            var content = "";

            if (culture == "ja")
            {
                content += name + "様、<br/><br/>店舗用（" + orgName + "）OSUSHI.APPへのご招待です。<br/><br/>";
                content += "--------------<br/>";
                content += memo + "<br/>";
                content += "--------------<br/><br/>";
                content += "下記のリンクをクリックしメールアドレス確認及び本登録とさせていただきます。<br/>";
                content += "<a href='" + url + "'>" + url + "</a><br/><br/>";
                content += "初期パスワードは下記の通りです、ログインした後にパスワードをご変更ください。<br/>";
                content += password + "<br/><br/>";
                content += Footer(culture);

                return content;
            }

            content += "Hello " + name + ", <br/><br/>You have invited to join (" + orgName + ") OSUSHI.APP<br/><br/>";
            content += "--------------<br/>";
            content += memo + "<br/>";
            content += "--------------<br/><br/>";
            content += "Please click on link below to confirm your registration.<br/>";
            content += "<a href='" + url + "'>" + url + "</a><br/>";
            content += "Your password is at below, please change to your own password once you login.<br/>";
            content += password + "<br/><br/>";
            content += Footer(culture);

            return content;
        }

        public static string Registereded(string culture, string url)
        {
            var content = "店舗用レシートローラーへの本登録が完了しました。。<br/><br/>";
            content += "下記のURLからレシートローラーをご利用ください。<br/>";
            content += "<a href='" + url + "'>" + url + "</a><br/>";
            content += Footer(culture);

            return content;
        }

        private static string Footer(string culture)
        {

            var content = "";
            if (culture == "ja")
            {
                content += "<br/><br/>|=============";
                content += "| BACKLOG by SEVENTH / バックログ - セブンスデータ";
                content += "| タスク管理サービス";
                content += "| https://backlog.7thdata.com/ja";
                content += "| support@7thdata.com";
                content += "|============= ";

                return content;
            }

            content += "<br/><br/>|=============";
            content += "| BACKLOG by SEVENTH";
            content += "| Manage your tasks.";
            content += "| https://backlog.7thdata.com/en";
            content += "| support@7thdata.com";
            content += "|============= ";

            return content;
        }
    }
}
